#pragma once
#include "Serial_handler.h"

Serial_handler::Serial_handler()
{
	baud_rate = 9600;
	config = "8N1";
	newline_format = "\r\n";
}

// Init serial with given config.
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

// Listens for serial input
void Serial_handler::serial_listener()
{
	if (Serial.available() > 0)
	{
		String input = Serial.readStringUntil('\n');

		if (input)
		{
			instruction_handler.insert_instruction(input);
		}
	}
}

// Reads serial until handshake received
// Returns false if what's received is not a propper handshake.
bool Serial_handler::handshake_approved()
{
	Json_container<JsonObject>* tmp;

	unsigned long start_time = millis();
	unsigned long timeout = 2000;

	while (millis() - start_time < timeout)
	{
		serial_listener();

		if (!instruction_handler.queue_is_empty())
		{
			tmp = instruction_handler.fetch_instruction();

			if (tmp->get()->containsKey("serial_handshake"))
			{
				if (tmp->get()->get<String>("serial_handshake") == "init")
				{
					send_handshake_response();
					delete tmp;
					return true;
				}
			}
			send_nack();
			delete tmp;
		}
	}

	return false;
}

// Reads serial until connection request received
// Returns false if what's received is not a propper request.
bool Serial_handler::connection_request_approved()
{
	Json_container<JsonObject>* tmp;
	JsonObject* nested_obj;

	unsigned long start_time = millis();
	unsigned long timeout = 2000;

	while (millis() - start_time < timeout)
	{
		serial_listener();

		if (!instruction_handler.queue_is_empty())
		{
			tmp = instruction_handler.fetch_instruction();

			if (tmp->get()->containsKey("connection_request"))
			{
				nested_obj = &tmp->get()->get<JsonVariant>("connection_request").asObject();

				if (config_approved(nested_obj->get<unsigned long>("baud_rate"), nested_obj->get<String>("config")))
				{
					newline_format = nested_obj->get<String>("newline");

					serial_init();
					delete tmp;
					return true;
				}
			}
			send_nack();
			delete tmp;
		}
	}

	return false;
}

bool Serial_handler::connection_init_approved()
{
	Json_container<JsonObject>* tmp;
	unsigned long start_time = millis();
	unsigned long timeout = 2000;

	send_ack();

	while (millis() - start_time < timeout)
	{
		serial_listener();

		if (!instruction_handler.queue_is_empty())
		{
			tmp = instruction_handler.fetch_instruction();

			if (tmp->get()->containsKey("connect"))
			{

				if (tmp->get()->get<String>("connect") == "ok")
				{
					delete tmp;
					return true;
				}
			}
			delete tmp;
		}
	}	
	return false;
}

bool Serial_handler::available_data_request_approved()
{
	Json_container<JsonObject>* tmp;
	unsigned long start_time = millis();
	unsigned long timeout = 2000;

	while (millis() - start_time < timeout)
	{
		serial_listener();

		if (!instruction_handler.queue_is_empty())
		{
			tmp = instruction_handler.fetch_instruction();

			if (tmp->get()->containsKey("request"))
			{

				if (tmp->get()->get<String>("request") == "available_data")
				{
					send_sensor_collection(sensor_container.get_available_sensors());
					delete tmp;
					return true;
				}
			}
			send_nack();
			delete tmp;
		}
	}

	return false;
}

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

void Serial_handler::send_handshake_response()
{
	Json_container<JsonObject>* handshake_response = json_handler.create_object();
	JsonObject& serial_handshake = handshake_response->get()->createNestedObject("serial_handshake");
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

	handshake_response->get()->printTo(Serial);
	Serial.print(newline_format);

	delete handshake_response;
}

//Sends ack to software layer
void Serial_handler::send_sensor_collection(LinkedList<String, Sensor*>& sensor_collection)
{
	Json_container<JsonObject>* ack = json_handler.create_object("serial_handshake", "ok");
	JsonArray& available_data = ack->get()->createNestedArray("available_data");

	for (int i = 0; i < sensor_collection.count(); i++)
	{
		Sensor* sensor = sensor_collection[i];
		for (int i = 0; i < sensor->get_data_count(); i++)
		{
			available_data.add(*json_handler.create_object(sensor->read_sensor()[i].name, "int")->get());
		}
	}

	ack->get()->printTo(Serial);
	Serial.print(newline_format);

	delete ack;
}

void Serial_handler::send_ack()
{
	Json_container<JsonObject>* ack = json_handler.create_object("connect", "init");

	ack->get()->printTo(Serial);
	Serial.print(newline_format);

	delete ack;
}

void Serial_handler::send_nack()
{
	Json_container<JsonObject>* nack = json_handler.create_object("serial_handshake", "failed");

	nack->get()->printTo(Serial);
	Serial.print(newline_format);

	delete nack;
}

void Serial_handler::print_to_serial(Json_container<JsonObject>* json)
{
	json->get()->printTo(Serial);
	Serial.print(newline_format);

	delete json;
}
