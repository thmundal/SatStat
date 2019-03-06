#pragma once
#include "./handlers/Serial_handler.h"

// Init input and output handler
Serial_handler serial_handler;
Instruction_handler instruction_handler;
Sensor_container sensor_container;

// Timing constrains
unsigned long sensor_interval_start_time = millis();
const unsigned long sensor_interval_duration = 1000;

unsigned long outer_timeout_start_time;
const unsigned long outer_timeout_duration = 10000;

void handshake()
{
	// Loop until handshake received
	while (true)
	{
		if (serial_handler.handshake_approved())
		{
			break;
		}
	}
}

bool connection()
{
	// Loop until connection request received
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

bool connection_init()
{
	// Init connection on new config
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

bool provide_sensor_data()
{
	// Init connection on new config
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

void setup()
{
	// Temp and hum sensor gnd and 5V
	pinMode(7, OUTPUT);
	digitalWrite(7, LOW);
	pinMode(5, OUTPUT);
	digitalWrite(5, HIGH);

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
}

void loop()
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