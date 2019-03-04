#pragma once
#include "./handlers/Serial_handler.h"
#include "./other/Sensor_container.h"

// Init input and output handler
Serial_handler* serial_handler;
Instruction_handler* instruction_handler;
Sensor_container* sensor_container;

// Timing constrains
unsigned long start_time = millis();
const int duration = 1000;

void handshake()
{
	// Loop until handshake received
	while (true)
	{
		// Send ack if approved, nack if not
		if (serial_handler->handshake_approved())
		{
			break;
		}
	}
}

bool connection()
{
	// Loop until connection request received
	unsigned long start_time = millis();
	unsigned long timeout = 10000;

	while (millis() - start_time < timeout)
	{
		// Send nack if not approved
		if (serial_handler->connection_request_approved())
		{	
			return true;
		}
	}

	return false;
}

bool connection_init()
{
	// Init connection on new config
	unsigned long start_time = millis();
	unsigned long timeout = 10000;

	while (millis() - start_time < timeout)
	{		
		if (serial_handler->connection_init_approved())
		{
			return true;
		}
	}

	return false;
}

bool provide_sensor_data()
{
	// Init connection on new config
	unsigned long start_time = millis();
	unsigned long timeout = 10000;

	while (millis() - start_time < timeout)
	{
		// Send nack if not approved
		if (serial_handler->available_data_request_approved())
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

	serial_handler = new Serial_handler();
	instruction_handler = new Instruction_handler();
	sensor_container = new Sensor_container();

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
		serial_handler->send_nack();
		delay(30);
		Serial.begin(9600);
	}				
}

void loop()
{	
	serial_handler->serial_listener();

	if (!instruction_handler->queue_is_empty())
	{				
		instruction_handler->interpret_instruction();
	}	
	
	if (instruction_handler->sadm_auto_rotate_en())
	{
		instruction_handler->sadm_auto_rotate();
	}

	// Runs with an interval equal to the duration
	if (!(millis() - start_time < duration))
	{	
		// Prints sensor data
		serial_handler->print_to_serial(sensor_container->read_sensors());

		// Update start time to current time
		start_time = millis();
	}
}