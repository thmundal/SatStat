#pragma once
#include "input_handler.h"
#include "output_handler.h"
#include "sensor_controller.h"
#include "Stepper.h"
#include "dht.h"
#include "TimedAction.h"

const int stepsPerRev = 32;

// IN1 - IN4 på pin 8 - 11
Stepper myStepper(stepsPerRev, 8, 10, 9, 11);

int steps;
float factor = 3.25;

uint8_t* serialBuffer;

int sensorPin = 7;
dht DHT;

TimedAction tempCheck = TimedAction(500, CheckTemperature);
TimedAction panels = TimedAction(1000, MovePanels);


void setup() {
	// initialize the serial port:
	Serial.begin(115200);
	myStepper.setSpeed(700);
}

void CheckTemperature() {
	int check = DHT.read11(sensorPin);
	Serial.println(DHT.temperature);
}

void MovePanels() {
	steps = (int)(1024 * factor);
	myStepper.step(steps);
	myStepper.step(-steps);
	delay(500);
	myStepper.step(-steps);
	myStepper.step(steps);
	delay(500);
}

void loop() {
	//if (Serial.readBytesUntil('\n', serialBuffer, 8) > 0) {
	//	Serial.println(*serialBuffer);
	//}
	//else {
	//	Serial.println("No serial input received");
	//}

	tempCheck.check();
	panels.check();
}