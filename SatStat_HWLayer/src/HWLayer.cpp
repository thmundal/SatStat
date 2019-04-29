#pragma once
#include "HWLayer.h"

/**
*	Constructor instantiating the serial_handler member.
*/
HWLayer::HWLayer()
{
	serial_handler = new Serial_handler(sensor_container, instruction_handler);
}

/**
*	Destructor deleting the serial_handler member.
*/
HWLayer::~HWLayer()
{
	delete serial_handler;
}

/**
*	Sets up Vcc and GND pins for DHT22 temperature and humidity sensor, and initializes the serial port.
*	Also handles the handshake protocol as defined in SatStat communication protocol.
*/
void HWLayer::setup()
{
	// DHT22 GND and Vcc
	pinMode(2, OUTPUT);
	digitalWrite(2, LOW);
	pinMode(3, OUTPUT);
	digitalWrite(3, HIGH);

	// Init serial
	Serial.begin(9600);

	// Execute handshake protocol
	while (true)
	{
		handshake();

		if (connection())
		{
			if (connection_init())
			{
				if (provide_available_data())
				{
					if (provide_available_instructions())
					{
						break;
					}
				}
			}
		}
		serial_handler->send_nack();
		delay(30);
		Serial.begin(9600);
	}

	// Init sensor interval start time
	sensor_interval_start_time = millis();
}

/**
*	Continuously listens for serial inputs, prints read sensor data to serial at a given interval and execute received instructions.
*/
void HWLayer::loop()
{
	// Infinite while loop to prevent continuous calls to this methods from the loop function in SatStat_HWLayer.ino
	while (true)
	{
		// Listen for input on serial port
		serial_handler->serial_listener();

		// Checks if there is no function currently executing and if the queue currently holds instructions
		if (Function_control::is_available() && !instruction_handler.queue_is_empty())
		{
			// Executes the first instruction in the queue
			instruction_handler.interpret_instruction();
		}
		
		// Continuously executes the currently loaded instruction
		Function_control::run();	
	
		// Calls auto rotate if it's enabled
		if (SADM_functions::get_auto_rotate_en())
		{
			SADM_functions::auto_rotate();
		}

		// Runs with an interval equal to the duration
		if (!(millis() - sensor_interval_start_time < sensor_interval_duration))
		{
			// Reads the sensors
			sensor_container.read_all_sensors();
						
			// Fetches subscribed sensor data
			auto sub_data = sensor_container.get_sub_data();

			// Prints subscribed sensor data
			if (sub_data->size() > 0)
			{
				serial_handler->print_to_serial(sub_data);
			}
		
			// Update sensor interval start time to current time
			sensor_interval_start_time = millis();
		}
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
		if (serial_handler->handshake_approved())
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
		if (serial_handler->connection_request_approved())
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
		if (serial_handler->connection_init_approved())
		{
			return true;
		}
	}

	return false;
}

/**
*	Loops until available data request is received, or a timeout occur.
*/
bool HWLayer::provide_available_data()
{
	outer_timeout_start_time = millis();

	while (millis() - outer_timeout_start_time < outer_timeout_duration)
	{
		if (serial_handler->request_approved("available_data"))
		{
			serial_handler->send_available_data();
			return true;
		}
	}

	return false;
}

/**
*	Loops until available instructions request is received, or a timeout occur.
*/
bool HWLayer::provide_available_instructions()
{
	outer_timeout_start_time = millis();

	while (millis() - outer_timeout_start_time < outer_timeout_duration)
	{
		if (serial_handler->request_approved("available_instructions"))
		{
			serial_handler->send_available_instructions();
			return true;
		}
	}

	return false;
}