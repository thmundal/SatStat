#pragma once
#include "Temp_sensor.h"
#include "input_handler.h"
#include "output_handler.h"

input_handler in;
output_handler out;
StaticJsonBuffer<200>* jsonBuffer = new StaticJsonBuffer<200>;
JsonObject* root = &jsonBuffer->createObject();

//const int stepsPerRev = 32;
//
//// IN1 - IN4 på pin 8 - 11
//Stepper myStepper(stepsPerRev, 8, 10, 9, 11);
//
//int steps = 0;
//bool dir = false;
//const float factor = 3.25;
//const int step_limit = (int)(1024 * factor);
//
//uint8_t* serialBuffer;
//
//int sensorPin = 7;
//dht DHT;
//int check;

unsigned long start_time = millis();
const int duration = 1000;

bool auto_rotate = false;

void setup() {
	// initialize the serial port:
	Serial.begin(9600);
	//myStepper.setSpeed(700);

	// Jon testing stuff	
	in.listener();
}

//void AutoRotatePanels() {
//	if (steps < step_limit)
//	{
//		steps++;
//	}
//	else
//	{
//		dir = !dir;
//		steps = 0;
//	}
//
//	if (!dir)
//	{
//		myStepper.step(1);
//	}
//	else
//	{
//		myStepper.step(-1);
//	}
//}

void loop() {
	//if (auto_rotate) {
	//	AutoRotatePanels();
	//}
	
	in.listener();

	if (!(millis() - start_time < duration))
	{    
		//CheckAndSendTemperature();
		//ReadSerialInput();		

		in.read_sensor("temp"); // this does absolutly nothing
		out.print_to_serial(in.get_instruction());
		start_time = millis();
	}
}
