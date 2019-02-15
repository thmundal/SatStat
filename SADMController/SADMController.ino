#pragma once
#include "input_handler.h"
#include "output_handler.h"
#include "Stepper.h"
#include "dht.h"

input_handler test;
StaticJsonBuffer<200>* jsonBuffer = new StaticJsonBuffer<200>;
JsonObject* root = &jsonBuffer->createObject();

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

bool auto_rotate = false;

void setup() {
	// initialize the serial port:
	Serial.begin(9600);
	myStepper.setSpeed(700);

	// Jon testing stuff	
	test.listener();
}

void AutoRotatePanels() {
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
}

void ReadSerialInput() {
	if (Serial.available() > 0) {
		String input = Serial.readStringUntil('\n');
		if (input) {
			//Serial.println(*serialBuffer);

			if (input == "auto_start") {
				auto_rotate = true;
			}
			else if (input == "auto_stop") {
				auto_rotate = false;
			}
		}
		else {
			//Serial.println("No serial input received");
		}
	}
}

void CheckAndSendTemperature() {
	check = DHT.read11(sensorPin);
	String out = (String) DHT.temperature;
	out.replace(".", ",");
	//Serial.println("This is a string bleh");
	Serial.println("{\"temperature\":\"" + out + "\"}");
	//Serial.println(DHT.temperature);
}

void loop() {
	//if (auto_rotate) {
	//	AutoRotatePanels();
	//}
 // 
	//if (!(millis() - start_time < duration))
	//{    
	//	CheckAndSendTemperature();
	//	ReadSerialInput();

	//	start_time = millis();
	//}

	if (test.instruction_count() > 0)
	{	
		root = test.fetch_instruction();
		const char* sensor = (*root)["sensor"];
		long time = (*root)["time"];
		double latitude = (*root)["data"][0];
		double longitude = (*root)["data"][1];
		Serial.println(sensor);
		Serial.println(time);
		Serial.println(latitude);
		Serial.println(longitude);

	}
  
}
