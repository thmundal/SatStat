#pragma once
#include "input_handler.h"
#include "output_handler.h"
#include "sensor_controller.h"
#include "Stepper.h"
#include "dht.h"

const int stepsPerRev = 32;

// IN1 - IN4 på pin 8 - 11
Stepper myStepper(stepsPerRev, 8, 10, 9, 11);

int steps = 0;
bool dir = false;
const float factor = 3.25;
const int step_limit = (int)(1024 * factor);

uint8_t* serialBuffer;

int sensorPin = 7;
dht DHT;
int check;

unsigned long start_time = millis();
const int duration = 1000;

void setup() {
	// initialize the serial port:
	Serial.begin(115200);
	myStepper.setSpeed(700);
}

void loop() {
  if (steps < step_limit)
  {
    steps++;
  }
  else
  {
    dir = !dir;
    steps = 0;
  }
  
  if (!dir)
  {
    myStepper.step(1); 
  }
  else
  {
    myStepper.step(-1);
  }
  
  if (!(millis() - start_time < duration))
  {    
    check = DHT.read11(sensorPin);
    Serial.println(DHT.temperature);
    start_time = millis();
  }  
  
	//if (Serial.readBytesUntil('\n', serialBuffer, 8) > 0) {
	//	Serial.println(*serialBuffer);
	//}
	//else {
	//	Serial.println("No serial input received");
	//}
  
}
