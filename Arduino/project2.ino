#define CLK_PIN 12  
#define DT_PIN 13 
const int x_pot = A0; 
const int y_pot = A1; 
const int SW_Pin = 2;

int counter = 0;
int lastCounter = 0;
int currentStateCLK;
int lastStateCLK; 
int buttonState = HIGH; 
int lastButtonState = HIGH; 

unsigned long lastJoystickTime = 0;
const unsigned long joystickInterval = 10;

void setup() { 
  Serial.begin(9600); 
  pinMode(CLK_PIN, INPUT_PULLUP); 
  pinMode(DT_PIN, INPUT_PULLUP); 
  pinMode(x_pot, INPUT); 
  pinMode(y_pot, INPUT); 
  pinMode(SW_Pin, INPUT_PULLUP); 
  lastStateCLK = digitalRead(CLK_PIN);
} 

void loop() { 
  currentStateCLK = digitalRead(CLK_PIN);
  
  if (currentStateCLK != lastStateCLK && currentStateCLK == LOW) {
    if (digitalRead(DT_PIN) == HIGH) {
      counter--;
    } else {
      counter++;
    }
  }
  lastStateCLK = currentStateCLK;
  if (counter != lastCounter) {
    Serial.println(counter);
    lastCounter = counter;
  }
  buttonState = digitalRead(SW_Pin); 
  if (buttonState != lastButtonState) { 
    if (buttonState == LOW) { 
      Serial.println("ACTIVE"); 
    } else { 
      Serial.println("NONACTIVE"); 
    } 
    lastButtonState = buttonState; 
  } 

  if (millis() - lastJoystickTime >= joystickInterval) {
    lastJoystickTime = millis();
    int current_y = analogRead(y_pot); 
    int current_x = analogRead(x_pot); 
    if (current_y > 1000) { 
      Serial.println("DOWN"); 
    } else if (current_x > 1000) { 
      Serial.println("RIGHT"); 
    } else if (current_y < 50) { 
      Serial.println("UP"); 
    } else if (current_x < 50) { 
      Serial.println("LEFT"); 
    }
  }
  
}