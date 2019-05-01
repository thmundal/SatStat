#pragma once
#include "Message_handler.h"
#include "../containers/Sensor_container.h"

/**
*	The Serial_handler class is responsible for establishing connection with a client through the serial port,
*	as well as listening for input and printing output through the serial port when a connection has been established.
*	It has methods complementing the handshake protocol defined in the SatStat communication protocol document.
*/
class Serial_handler
{
public:	
	Serial_handler(Sensor_container& sc, Message_handler& ih);
	
	void handshake();

	void send_available_data();
	void send_available_instructions();

	void serial_listener();

	void send_ping();	

	void print_to_serial(Json_container<JsonObject>& json);

private:		
	bool handshake_approved();
	bool connection_request_approved();
	bool connection_init_approved();	

	bool config_approved(const unsigned long& baud_rate, const String& config);
	void serial_init();

	void send_handshake_response();
	void send_ack();
	void send_error_message(const String& msg);

	Sensor_container* sensor_container;
	Message_handler* message_handler;
	
	unsigned long m_start_time;
	const unsigned long m_timeout_duration = 1000;
	int m_timeout_counter;

	unsigned long baud_rate;
	String config;
	String newline_format;
	
};