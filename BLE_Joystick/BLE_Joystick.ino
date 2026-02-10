#include <BleGamepad.h>

// Configuración del dispositivo
BleGamepad bleGamepad("ESP32-C3 Joystick", "Proyectos Francisco", 100);

// Definición de pines
const int pinX = 0;
const int pinY = 1;
const int pinButton = 2;

// Variables para el botón e interrupción
volatile bool isPressed = false;
unsigned long lastInterruptTime = 0;

// Variables para la calibración automática
int centroX = 2048; 
int centroY = 2048;
int deadzone = 150; // Margen para evitar que el cursor tiemble en el centro

// Función de interrupción (ISR)
void IRAM_ATTR handleButton() {
  unsigned long currentTime = millis();
  if (currentTime - lastInterruptTime > 25) { // Anti-rebote
    // REGLA DE ORO: isPressed debe ser IGUAL al estado físico del pin
    // Si el pin está en LOW (presionado), isPressed es TRUE.
    // Si el pin está en HIGH (suelto), isPressed es FALSE.
    isPressed = (digitalRead(pinButton) == LOW); 
    
    lastInterruptTime = currentTime;
  }
}

void setup() {
  Serial.begin(115200);
  
  // Configuración de pines
  pinMode(pinButton, INPUT_PULLUP); 
  attachInterrupt(digitalPinToInterrupt(pinButton), handleButton, CHANGE);

  // --- PROCESO DE CALIBRACIÓN ---
  // Es vital no tocar el joystick durante este segundo
  Serial.println("Calibrando ejes... No muevas el joystick.");
  delay(1000); 
  centroX = analogRead(pinX);
  centroY = analogRead(pinY);
  Serial.printf("Calibración lista: CentroX=%d, CentroY=%d\n", centroX, centroY);
  // ------------------------------

  bleGamepad.begin();
  Serial.println("Buscando conexión Bluetooth...");
}

void loop() {
  if (bleGamepad.isConnected()) {
    // 1. Lectura de valores crudos
    int rawX = analogRead(pinX);
    int rawY = analogRead(pinY);

    // 2. Mapeo inteligente con zona muerta
    int outX, outY;

    // Eje X: Si está cerca del centro calibrado, forzamos el centro real (16384)
    if (abs(rawX - centroX) < deadzone) {
      outX = 16384;
    } else {
      outX = (rawX < centroX) ? map(rawX, 0, centroX, 0, 16384) : map(rawX, centroX, 4095, 16384, 32767);
    }

    // Eje Y: Lo mismo para el eje Y
    if (abs(rawY - centroY) < deadzone) {
      outY = 16384;
    } else {
      outY = (rawY < centroY) ? map(rawY, 0, centroY, 0, 16384) : map(rawY, centroY, 4095, 16384, 32767);
    }

    // 3. Enviar datos de los ejes
    bleGamepad.setAxes(outX, outY, 0, 0, 0, 0, 0, 0);

    // 4. Enviar estado del botón
    if (isPressed) {
      bleGamepad.press(BUTTON_9);
    } else {
      bleGamepad.release(BUTTON_9);
    }
  }

  delay(10); // 100Hz de refresco
}