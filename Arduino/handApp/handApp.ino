#include "BluetoothSerial.h"
#include "String.h"

BluetoothSerial SerialBT;

const byte numChars = 32;
char receivedChars[numChars];   // an array to store the received data

boolean newData = false;

int result;

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
    // Grasps
    if (strncmp(receivedChars, "large", 5) == 0){
      Serial.write("1");    
      Serial.flush();  
    } else if (strncmp(receivedChars, "small", 5) == 0){
      Serial.write("2");     
      Serial.flush();       
    } else if (strncmp(receivedChars, "palmar", 6) == 0){
      Serial.write("3");     
      Serial.flush();
    } else if (strncmp(receivedChars, "ball", 4) == 0){
      Serial.write("4");     
      Serial.flush();
    } else if (strncmp(receivedChars, "lateral", 7) == 0){
      Serial.write("5");     
      Serial.flush();
    } else if (strncmp(receivedChars, "thumb1", 6) == 0){
      Serial.write("6");     
      Serial.flush();
    } else if (strncmp(receivedChars, "thumb2", 6) == 0){
      Serial.write("7");     
      Serial.flush();
    } else if (strncmp(receivedChars, "thumb3", 6) == 0){
      Serial.write("8");     
      Serial.flush();
    } else if (strncmp(receivedChars, "resetgrip", 9) == 0){
      Serial.write("9");
      Serial.flush();
      
    // Resets
    } else if (strncmp(receivedChars, "resetlarge", 10) == 0){
      // 97
      Serial.write("a");    
      Serial.flush();  
    } else if (strncmp(receivedChars, "resetsmall", 10) == 0){
      // 98
      Serial.write("b");     
      Serial.flush();           
    } else if (strncmp(receivedChars, "resetpalmar", 11) == 0){
      // 99
      Serial.write("c");     
      Serial.flush();
    } else if (strncmp(receivedChars, "resetball", 9) == 0){
      // 100
      Serial.write("d");     
      Serial.flush();
    } else if (strncmp(receivedChars, "resetlateral", 12) == 0){
      // 101
      Serial.write("e");     
      Serial.flush();
    } else if (strncmp(receivedChars, "resetthumb1", 11) == 0){
      // 102
      Serial.write("f");     
      Serial.flush();
    } else if (strncmp(receivedChars, "resetthumb2", 11) == 0){
      // 103
      Serial.write("g");     
      Serial.flush();
    } else if (strncmp(receivedChars, "resetthumb3", 11) == 0){
      // 104
      Serial.write("h");     
      Serial.flush();
    } else if (strncmp(receivedChars, "handwave", 8) == 0){
      // 105
      Serial.write("i");
      Serial.flush();
    }


      newData = false;
  }
}
