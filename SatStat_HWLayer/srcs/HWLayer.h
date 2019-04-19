#pragma once
#include "./handlers/Serial_handler.h"

/**
*	This class acts as the "main" class.
*	Doxygen, the documentation generation tool, doesn't support .ino files,
*	therefore this class has been created to be able to generate documentation for the "main" class.
*	The project still has a .ino file for it to be considered an arduino project,
*	but the setup and loop functions in that file simply calls the corresponding methods in this one.
*	The setup method of this class handles the handshake protocol, where as the loop method handles the receiving of instructions from serial,
*	transmition of sensor data through serial, and execution of instructions.
*/
class HWLayer
{
public:
	// Main methods
	void setup();
	void loop();

private:
	// Handshake protocol methods
	void handshake();
	bool connection();
	bool connection_init();
	bool provide_sensor_data();

	// Init input and output handler
	Serial_handler serial_handler;
	Instruction_handler instruction_handler;
	Sensor_container sensor_container;

	// Timing constrains
	unsigned long sensor_interval_start_time;
	const unsigned long sensor_interval_duration = 2000;

	unsigned long outer_timeout_start_time;
	const unsigned long outer_timeout_duration = 10000;
};