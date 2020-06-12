#include "BluetoothSerial.h"
#include "String.h"

BluetoothSerial SerialBT;

const byte numChars = 32;
char receivedChars[numChars];   // an array to store the received data

boolean newData = false;

int result;

/*
 *  When using Bluetooth, ADC2 is unavailable to be used
 */
// Pins for Joystick (ADC1)
const int joyX = 32;
const int joyY = 33;
const int joySW = 34;

int xValue, yValue, swValue;

void callback(esp_spp_cb_event_t event, esp_spp_cb_param_t *param){
  if(event == ESP_SPP_SRV_OPEN_EVT){
    Serial.println("Client Connected");
  } 

  if(event == ESP_SPP_CLOSE_EVT){
    Serial.println("Client Disconnected");
  }
}

void setup() {
  // Serial for Android
  Serial.begin(115200);
  SerialBT.register_callback(callback);

  if(!SerialBT.begin("ESP32")){
    Serial.println("An error occured initializing Bluetooth");
  } else {
    Serial.println("Bluetooth initialized");
  }
  
  Serial.println("The device started, now you can pair it with bluetooth!");
}

void loop() {
  xValue = analogRead(joyX);
  yValue = analogRead(joyY);
  swValue = analogRead(joySW);

  // X Values
  if(2000 < xValue){
    // Left
    Serial.write("1");
//    Serial.println(xValue);
    Serial.flush();
    delay(20);
  } else if (xValue < 1500){
    // Right
    Serial.write("2");
//    Serial.println(xValue);
    Serial.flush(); 
    delay(20);
  } 

  // Y Values
  if(1940 < yValue){
    // Up
    Serial.write("3");
//    Serial.println(yValue);
    Serial.flush();
    delay(20);
  } else if (yValue < 1520){
    // Down
    Serial.write("4");
//    Serial.println(yValue);
    Serial.flush(); 
    delay(20);
  }

   
  if (Serial.available()) {
    SerialBT.write(Serial.read());
  }

  recvWithEndMarker();
  showNewData();
}

void recvWithEndMarker() {
  static byte ndx = 0;
  char endMarker = '\n';
  char rc;

  while (SerialBT.available() > 0 && newData == false) {
    rc = SerialBT.read();

    if (rc != endMarker) {
      receivedChars[ndx] = rc;
      ndx++;

      if (ndx >= numChars) {
        ndx = numChars - 1;
      }
    } else {
      receivedChars[ndx] = '\0';
      ndx = 0;
      newData = true;
    }
  }
}

void showNewData() {
  if (newData == true) {
    if (strncmp(receivedChars, "Shoulder", 8) == 0){
//      Serial.print("Shoulder: ");
//      Serial.println(receivedChars); 
      Serial.write("5");    
      Serial.flush();  
    } else if (strncmp(receivedChars, "Forearm", 7) == 0){
//      Serial.print("Forearm: ");
//      Serial.println(receivedChars); 
      Serial.write("6");     
      Serial.flush();           
    } else if (strncmp(receivedChars, "Wrist", 5) == 0){
//      Serial.print("Wrist: ");
//      Serial.println(receivedChars); 
      Serial.write("7");       
      Serial.flush();         
    } else if (strncmp(receivedChars, "Fingers", 7) == 0){
//      Serial.print("Fingers: ");
//      Serial.println(receivedChars);
      Serial.write("8");      
      Serial.flush();   
    } else {
//      Serial.println(receivedChars);
//      Serial.println(sizeof(receivedChars));
    }
//    result = strncmp(receivedChars, "Forearm", 7);
//    Serial.println(result);
    
    newData = false;
  }
}
