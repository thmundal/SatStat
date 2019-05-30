#pragma once
#include "./handlers/Serial_handler.h"
#include "../dbg/Log.h"

/**
*	This class acts as the "main" class.
*	Doxygen, the documentation generation tool, doesn't support .ino files,
*	therefore this class has been created to be able to generate documentation for the "main" class.
*	The project still has a .ino file for it to be considered an Arduino project,
*	but the setup and loop functions in that file simply calls the corresponding methods in this one.
*	The setup method of this class handles the handshake protocol, where as the loop method handles the receiving of messages from serial,
*	transmition of sensor data through serial, and execution of messages.
*/
class HWLayer
{
public:	
	// Destructor
	~HWLayer();

	// Main methods
	void setup();
	void loop();

private:
	// Containers and handlers
	Sensor_container sensor_container;
	Message_handler message_handler;
	Serial_handler* serial_handler;

	// Timing constrains
	unsigned long sensor_interval_start_time;
	const unsigned long sensor_interval_duration = 3;
};