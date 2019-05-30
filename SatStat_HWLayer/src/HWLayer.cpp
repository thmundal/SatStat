#pragma once
#include "HWLayer.h"

// ISR variables for manual override
volatile const int interrupt_pin = 2;
volatile unsigned long last_edge = millis();
volatile const unsigned long bounce_time = 50;
volatile bool manual_override = false;

// Function declaration for isr
void mode_switch();

/**
*	Destructor deleting the serial_handler member.
*/
HWLayer::~HWLayer()
{
	if (serial_handler != nullptr)
	{
		delete serial_handler;
	}
}

/**
*	Sets up Vcc and GND pins for DHT22 temperature and humidity sensor, and initializes the serial port.
*	Also handles the handshake protocol as defined in SatStat communication protocol.
*/
void HWLayer::setup()
{
	// Init serial
	Serial.begin(9600);

	// Mode selection pinout for isr
	pinMode(interrupt_pin, INPUT);
	attachInterrupt(digitalPinToInterrupt(interrupt_pin), mode_switch, CHANGE);

	// Stepper motor pinout
	SADM_functions::init();	
	
	// Instantiating serial_handler
	serial_handler = new Serial_handler(sensor_container, message_handler);

	// Execute handshake protocol
	serial_handler->handshake();

	// Send available
	serial_handler->send_available_data();
	serial_handler->send_available_instructions();

	// Init sensor interval start time
	sensor_interval_start_time = millis();
}

/**
*	Continuously listens for serial inputs, prints read sensor data to serial at a given interval and execute received messages.
*/
void HWLayer::loop()
{
	// Infinite while loop to prevent continuous calls to this methods from the loop function in SatStat_HWLayer.ino
	while (true)
	{
		// Runs when not manual override
		while (!manual_override)
		{
			// Listen for input on serial port
			serial_handler->serial_listener();

			// Checks if there is no function currently executing and if the queue currently holds instructions
			if (Function_control::is_available() && message_handler.has_instructions())
			{
				// Executes the first instruction in the queue
				message_handler.interpret_instruction();
			}

			// Checks if the queue currently holds requests
			if (message_handler.has_requests())
			{
				// Executes the first request in the queue
				message_handler.interpret_request();
			}
		
			// Continuously executes the currently loaded instruction
			Function_control::run();

			if (millis() - sensor_interval_start_time > sensor_interval_duration)
			{
				//scoped_timer;
				// Reads the sensors
				sensor_container.read_all_sensors();

				// Fetches subscribed sensor data
				auto sub_data = sensor_container.get_sub_data();

				// Prints subscribed sensor data if any, else send ping
				if (sub_data->size() > 0)
				{
					serial_handler->print_to_serial(sub_data);
				}
				else
				{
					serial_handler->send_ping();
				}
				sensor_interval_start_time = millis();
			}
		}
	}
}

// Function definition for isr
void mode_switch()
{
	if (millis() - last_edge > bounce_time)
	{
		manual_override = !manual_override;
		last_edge = millis();
	}
}