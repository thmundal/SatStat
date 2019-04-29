#pragma once
#include "Serial_handler.h"

/**
*	Constructor taking a reference to a Sensor_container and an Instruction_handler.
*	Sets all the members.
*/
Serial_handler::Serial_handler(Sensor_container& sc, Instruction_handler& ih)
{
	baud_rate = 9600;
	config = "8N1";
	newline_format = "\r\n";

	sensor_container = &sc;
	instruction_handler = &ih;
}

/**
*	Initializes serial with configuration stored in the member variables baud_rate and config.
*/
void Serial_handler::serial_init()
{
	if (config == "5N1") { Serial.begin(baud_rate, SERIAL_5N1); }
	else if (config == "6N1") { Serial.begin(baud_rate, SERIAL_6N1); }
	else if (config == "7N1") { Serial.begin(baud_rate, SERIAL_7N1); }
	else if (config == "8N1") { Serial.begin(baud_rate, SERIAL_8N1); }
	else if (config == "5N2") { Serial.begin(baud_rate, SERIAL_5N2); }
	else if (config == "6N2") { Serial.begin(baud_rate, SERIAL_6N2); }
	else if (config == "7N2") { Serial.begin(baud_rate, SERIAL_7N2); }
	else if (config == "8N2") { Serial.begin(baud_rate, SERIAL_8N2); }
	else if (config == "5E1") { Serial.begin(baud_rate, SERIAL_5E1); }
	else if (config == "6E1") { Serial.begin(baud_rate, SERIAL_6E1); }
	else if (config == "7E1") { Serial.begin(baud_rate, SERIAL_7E1); }
	else if (config == "8E1") { Serial.begin(baud_rate, SERIAL_8E1); }
	else if (config == "5E2") { Serial.begin(baud_rate, SERIAL_5E2); }
	else if (config == "6E2") { Serial.begin(baud_rate, SERIAL_6E2); }
	else if (config == "7E2") { Serial.begin(baud_rate, SERIAL_7E2); }
	else if (config == "8E2") { Serial.begin(baud_rate, SERIAL_8E2); }
	else if (config == "5O1") { Serial.begin(baud_rate, SERIAL_5O1); }
	else if (config == "6O1") { Serial.begin(baud_rate, SERIAL_6O1); }
	else if (config == "7O1") { Serial.begin(baud_rate, SERIAL_7O1); }
	else if (config == "8O1") { Serial.begin(baud_rate, SERIAL_8O1); }
	else if (config == "5O2") { Serial.begin(baud_rate, SERIAL_5O2); }
	else if (config == "6O2") { Serial.begin(baud_rate, SERIAL_6O2); }
	else if (config == "7O2") { Serial.begin(baud_rate, SERIAL_7O2); }
	else if (config == "8O2") { Serial.begin(baud_rate, SERIAL_8O2); }
}

/**
*	Listens for serial input. Has to be continuously called to store the input as soon as it's available.
*/
void Serial_handler::serial_listener()
{
	if (Serial.available() > 0)
	{
		String input = Serial.readStringUntil('\n');

		if (input)
		{
			if (!instruction_handler->insert_instruction(input))
			{
				Json_container<JsonObject> tmp;
				tmp->set("Error", "Could not parse received data!");
				tmp->printTo(Serial);
			}
		}
	}
}

/**
*	Prints JSON formatted negative acknowledgement to serial.
*/
void Serial_handler::send_nack()
{
	Json_container<JsonObject> nack;
	nack->set("serial_handshake", "failed");
	nack->printTo(Serial);
	Serial.print(newline_format);
}

/**
*	Reads serial until handshake received, and sends handshake response according to SatStat communication protocol when it is.
*	Sends NACK and returns false if what's received is not a proper handshake.
*	If no input is received before timeout occurs, it returns false without sending NACK.
*/
bool Serial_handler::handshake_approved()
{
	Json_container<JsonObject> tmp;

	unsigned long start_time = millis();
	unsigned long timeout = 2000;

	while (millis() - start_time < timeout)
	{
		serial_listener();

		if (!instruction_handler->queue_is_empty())
		{
			tmp = instruction_handler->fetch_instruction();

			if (tmp->containsKey("serial_handshake"))
			{
				if (tmp->get<String>("serial_handshake") == "init")
				{
					send_handshake_response();
					return true;
				}
			}
			send_nack();
		}
	}

	return false;
}

/**
*	Reads serial until connection request received, and initializes serial with the received configuration.
*	Sends NACK and returns false if what's received is not a proper request.
*	If no input is received before timeout occurs, it returns false without sending NACK.
*/
bool Serial_handler::connection_request_approved()
{
	Json_container<JsonObject> tmp;	

	unsigned long start_time = millis();
	unsigned long timeout = 2000;

	while (millis() - start_time < timeout)
	{
		serial_listener();

		if (!instruction_handler->queue_is_empty())
		{
			tmp = instruction_handler->fetch_instruction();

			if (tmp->containsKey("connection_request"))
			{
				JsonObject& nested_obj = tmp->get<JsonVariant>("connection_request").asObject();

				if (config_approved(nested_obj.get<unsigned long>("baud_rate"), nested_obj.get<String>("config")))
				{
					newline_format = nested_obj.get<String>("newline");

					serial_init();
					return true;
				}
			}
			send_nack();
		}
	}

	return false;
}

/**
*	Reads serial until connection acknowledgement received.
*	Sends NACK and returns false if what's received is not a proper acknowledgement.
*	If no input is received before timeout occurs, it returns false without sending NACK.
*/
bool Serial_handler::connection_init_approved()
{
	Json_container<JsonObject> tmp;
	unsigned long start_time = millis();
	unsigned long timeout = 2000;

	send_ack();

	while (millis() - start_time < timeout)
	{
		serial_listener();

		if (!instruction_handler->queue_is_empty())
		{
			tmp = instruction_handler->fetch_instruction();

			if (tmp->containsKey("connect"))
			{

				if (tmp->get<String>("connect") == "ok")
				{
					return true;
				}
			}
			send_nack();
		}
	}	
	return false;
}

/**
*	Reads serial until the given request is received.
*	Sends NACK and returns false if what's received is not a proper request.
*	If no input is received before timeout occurs, it returns false without sending NACK.
*/
bool Serial_handler::request_approved(const String& req)
{
	Json_container<JsonObject> tmp;
	unsigned long start_time = millis();
	unsigned long timeout = 2000;

	while (millis() - start_time < timeout)
	{
		serial_listener();

		if (!instruction_handler->queue_is_empty())
		{
			tmp = instruction_handler->fetch_instruction();

			if (tmp->containsKey("request"))
			{

				if (tmp->get<String>("request") == req)
				{				
					return true;
				}
			}
			send_nack();
		}
	}

	return false;
}

/**
*	Prints the given Json_container<JsonObject> to the serial.
*/
void Serial_handler::print_to_serial(Json_container<JsonObject>& json)
{
	json->printTo(Serial);
	Serial.print(newline_format);
}

/**
*	Checks if the given configuration is valid.
*/
bool Serial_handler::config_approved(const unsigned long & baud_rate, const String& config)
{
	int data_bits = config.charAt(0);
	char parity = config.charAt(1);
	int stop_bits = config.charAt(2);

	if (data_bits >= 53 && data_bits <= 56)
	{
		if (parity == 'N' || parity == 'O' || parity == 'E')
		{
			if (stop_bits == 49 || stop_bits == 50)
			{
				if (baud_rate == 9600 || baud_rate == 14400 || baud_rate == 19200 || baud_rate == 28800 || baud_rate == 38400 || baud_rate == 57600 || baud_rate == 115200)
				{
					this->config = config;
					this->baud_rate = baud_rate;

					return true;
				}
			}
		}
	}

	return false;
}

/**
*	Creates a Json_container<JsonObject>, appends the available baud rates, configurations and newline formats and print it to the serial.
*/
void Serial_handler::send_handshake_response()
{
	Json_container<JsonObject> handshake_response;
	JsonObject& serial_handshake = handshake_response->createNestedObject("serial_handshake");
	JsonArray& baud_rates = serial_handshake.createNestedArray("baud_rates");
	JsonArray& configs = serial_handshake.createNestedArray("configs");
	JsonArray& newlines = serial_handshake.createNestedArray("newlines");

	for (unsigned long i = 9600; i <= 38400; i *= 2)
	{
		baud_rates.add(i);
	}


	for (unsigned long i = 14400; i <= 115200; i *= 2)
	{
		baud_rates.add(i);
	}

	String tmp;

	for (int i = 5; i <= 8; i++)
	{
		for (int j = 0; j < 3; j++)
		{
			for (int k = 1; k <= 2; k++)
			{
				tmp = String(i);

				switch (j)
				{
				case 0:
					tmp += 'N';
					break;
				case 1:
					tmp += 'O';
					break;
				case 2:
					tmp += 'E';
					break;
				default:
					break;
				}

				tmp += String(k);

				configs.add(tmp);
			}
		}
	}

	newlines.add("\r\n");
	newlines.add("\n");

	handshake_response->printTo(Serial);
	Serial.print(newline_format);
}

/**
*	Creates a Json_container<JsonObject>, appends the available data and prints it to the serial.
*/
void Serial_handler::send_available_data()
{
	Json_container<JsonObject> ack;	

	sensor_container->append_available_data(ack);

	ack->printTo(Serial);
	Serial.print(newline_format);
}

/**
*	Creates a Json_container<JsonObject>, appends the available instructions and prints it to the serial.
*/
void Serial_handler::send_available_instructions()
{
	Json_container<JsonObject> ack;

	instruction_handler->append_available_instructions(ack);

	ack->printTo(Serial);
	Serial.print(newline_format);
}

/**
*	Creates a Json_container<JsonObject>, appends the acknowledgement message and prints it to the serial.
*/
void Serial_handler::send_ack()
{
	Json_container<JsonObject> ack;
	ack->set("connect", "init");

	ack->printTo(Serial);
	Serial.print(newline_format);
}