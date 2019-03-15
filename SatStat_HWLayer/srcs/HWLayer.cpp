#pragma once
#include "HWLayer.h"

/**
*	Sets up Vcc and GND pins for DHT11 temperature and humidity sensor, and initializes the serial port.
*	Also handles the handshake protocol as defined in SatStat communication protocol.
*/
void HWLayer::setup()
{
	// Temp and hum sensor gnd and 5V
	pinMode(7, OUTPUT);
	digitalWrite(7, LOW);
	pinMode(5, OUTPUT);
	digitalWrite(5, HIGH);

	// Init serial
	Serial.begin(9600);

	while (true)
	{
		handshake();

		if (connection())
		{
			if (connection_init())
			{
				if (provide_sensor_data())
				{
					break;
				}
			}
		}
		serial_handler.send_nack();
		delay(30);
		Serial.begin(9600);
	}

	sensor_interval_start_time = millis();
}

/**
*	Continuously listens for serial inputs, prints read sensor data to serial at a given interval and execute received instructions.
*/
void HWLayer::loop()
{
	serial_handler.serial_listener();

	if (!instruction_handler.queue_is_empty())
	{
		instruction_handler.interpret_instruction();
	}

	if (instruction_handler.sadm_auto_rotate_en())
	{
		instruction_handler.sadm_auto_rotate();
	}

	// Runs with an interval equal to the duration
	if (!(millis() - sensor_interval_start_time < sensor_interval_duration))
	{
		// Prints sensor data
		serial_handler.print_to_serial(sensor_container.read_sensors());
		
		// Update start time to current time
		sensor_interval_start_time = millis();
	}
}

/**
*	Loops until serial handshake is received.
*	No timeout for this method as this is the root of the handshake protocol.
*	If an error occur at any other phase in the procol, it will reset to this point and wait for client to try again.
*/
void HWLayer::handshake()
{
	while (true)
	{
		if (serial_handler.handshake_approved())
		{
			break;
		}
	}
}

/**
*	Loops until connection request is received, or a timeout occur.
*/
bool HWLayer::connection()
{
	outer_timeout_start_time = millis();

	while (millis() - outer_timeout_start_time < outer_timeout_duration)
	{
		if (serial_handler.connection_request_approved())
		{
			return true;
		}
	}

	return false;
}

/**
*	Loops until connection acknowledgement is received, or a timeout occur.
*/
bool HWLayer::connection_init()
{
	outer_timeout_start_time = millis();

	while (millis() - outer_timeout_start_time < outer_timeout_duration)
	{
		if (serial_handler.connection_init_approved())
		{
			return true;
		}
	}

	return false;
}

/**
*	Loops until available sensor data request is received, or a timeout occur.
*/
bool HWLayer::provide_sensor_data()
{
	outer_timeout_start_time = millis();

	while (millis() - outer_timeout_start_time < outer_timeout_duration)
	{
		if (serial_handler.available_data_request_approved())
		{
			return true;
		}
	}

	return false;
}