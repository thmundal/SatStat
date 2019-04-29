#pragma once
#include "Instruction_handler.h"
#include "../containers/Sensor_container.h"

/**
*	The Serial_handler class is responsible for establishing connection with a client through the serial port,
*	as well as listening for input and printing output through the serial port when a connection has been established.
*	It has methods complementing the handshake protocol defined in the SatStat communication protocol document.
*/
class Serial_handler
{
public:
	// Constructor
	Serial_handler(Sensor_container& sc, Instruction_handler& ih);

	// Init methods
	void serial_init();
	void serial_listener();

	void send_nack();

	// Handshake methods
	bool handshake_approved();
	bool connection_request_approved();
	bool connection_init_approved();
	bool request_approved(const String& req);	
	void send_available_data();
	void send_available_instructions();

	void print_to_serial(Json_container<JsonObject>& json);

private:	
	bool config_approved(const unsigned long& baud_rate, const String& config);

	void send_handshake_response();
	void send_ack();

	unsigned long baud_rate;
	String config;
	String newline_format;

	Sensor_container* sensor_container;
	Instruction_handler* instruction_handler;
};