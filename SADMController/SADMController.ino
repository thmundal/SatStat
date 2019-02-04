/*
 Name:		SADMController.ino
 Created:	2/4/2019 12:36:47 PM
 Author:	thmun
*/

#include <Stepper.h>

const int stepsPerRevolution = 200;  // change this to fit the number of steps per revolution
// for your motor

// initialize the stepper library on pins 8 through 11:
Stepper myStepper(stepsPerRevolution, 8, 9, 10, 11);
uint8_t* serialBuffer;

void setup() {
	// set the speed at 60 rpm:
	//myStepper.setSpeed(60);
	// initialize the serial port:
	Serial.begin(9600);
}

void loop() {
	if (Serial.readBytesUntil('\n', serialBuffer, 8) > 0) {
		Serial.println(*serialBuffer);
	}
	else {
		Serial.println("No serial input received");
	}


	// step one revolution  in one direction:
	//Serial.println("clockwise");
	//myStepper.step(stepsPerRevolution);
	//delay(500);

	//// step one revolution in the other direction:
	//Serial.println("counterclockwise");
	//myStepper.step(-stepsPerRevolution);
	//delay(500);
}