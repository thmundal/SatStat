#pragma once
#include "Arduino.h"
#include "Instruction_handler.h"
#include "../other/Sensor_container.h"

class Serial_handler
{
public:
	// Constructor
	Serial_handler();

	// Init methods
	void serial_init();
	void serial_listener();

	void send_nack();

	bool handshake_approved();
	bool connection_request_approved();
	bool connection_init_approved();
	bool available_data_request_approved();	

	void print_to_serial(Json_container<JsonObject>* json);		

private:
	bool config_approved(const unsigned long& baud_rate, const String& config);

	void send_handshake_response();
	void send_sensor_collection();
	void send_ack();

	unsigned long baud_rate;
	String config;
	String newline_format;
	
	Sensor_container sensor_container;
	Json_handler json_handler;
	Instruction_handler instruction_handler;
};