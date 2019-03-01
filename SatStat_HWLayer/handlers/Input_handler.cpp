#pragma once
#include "Input_handler.h"

// Instantiate sensors
Input_handler::Input_handler()
{	
	sensor_collection.append("temp_hum", new Temp_hum_sensor("temp_hum", 6));	
	newline_format = "\n";
}

// Delete every sensor in the sensor collection
Input_handler::~Input_handler()
{
	for (int i = 0; i < sensor_collection.count(); i++)
	{
		delete sensor_collection[i];
	}	

	//delete handshake_response;
}

// Reads serial until handshake received
// Returns false if what's received is not a propper handshake.
bool Input_handler::handshake_approved()
{
	Json_container<JsonObject>* tmp;

	while (true)
	{
		serial_listener();
		
		if (!json_handler.queue_is_empty())
		{			
			tmp = json_handler.fetch_instruction();

			if (tmp->get()->containsKey("serial_handshake"))
			{
				if (tmp->get()->get<String>("serial_handshake") == "init")
				{
					delete tmp;
					return true;
				}
			}

			delete tmp;
			return false;
		}
	}
}

// Reads serial until connection request received
// Returns false if what's received is not a propper request.
bool Input_handler::connection_request_approved()
{	
	Json_container<JsonObject>* tmp;
	JsonObject* nested_obj;

	while (true)
	{
		serial_listener();

		if (!json_handler.queue_is_empty())
		{
			tmp = json_handler.fetch_instruction();

			if (tmp->get()->containsKey("connection_request"))
			{
				nested_obj = &tmp->get()->get<JsonVariant>("connection_request").asObject();

				if (config_approved(nested_obj->get<unsigned long>("baud_rate"), nested_obj->get<String>("config")))
				{
					newline_format = nested_obj->get<String>("newline");

					delete tmp;
					return true;
				}
			}

			delete tmp;
			return false;
		}
	}
}

// Listens for serial input
void Input_handler::serial_listener()
{
	if (Serial.available() > 0)
	{
		String input = Serial.readStringUntil('\n');

		if (input)
		{
			json_handler.insert_instruction(input);
		}
	}
}

bool Input_handler::config_approved(const unsigned long & baud_rate, const String& config)
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

// Init serial with given config.
void Input_handler::serial_init()
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

bool Input_handler::init_connection()
{
	Json_container<JsonObject>* tmp;
	unsigned long start_time = millis();
	unsigned long timeout = 10000;

	while (millis() - start_time < timeout)
	{
		serial_listener();

		if (!json_handler.queue_is_empty())
		{
			tmp = json_handler.fetch_instruction();

			if (tmp->get()->containsKey("connect"))
			{				

				if (tmp->get()->get<String>("connect") == "ok")
				{
					delete tmp;
					return true;
				}
			}

			delete tmp;
			return false;
		}
	}
}

// Read given sensor
Json_container<JsonObject>* Input_handler::read_sensor(const String& name)
{
	Sensor* sensor = sensor_collection.get(name);
	const Result* result = sensor->read_sensor();
	const int count = sensor->get_data_count();
	Json_container<JsonObject>* obj = json_handler.create_object();	

	for (int i = 0; i < count; i++)
	{
		json_handler.append_to(obj, result[i].name, result[i].data);
	}

	return obj;
}

// Reads all sensors
Json_container<JsonObject>* Input_handler::read_sensors()
{	
	Sensor* sensor;
	int sensor_count = sensor_collection.count();
	const Result* result;
	int result_count;

	Json_container<JsonObject>* obj = json_handler.create_object();

	for (int i = 0; i < sensor_count; i++)
	{
		sensor = sensor_collection[i];
		result = sensor->read_sensor();
		result_count = sensor->get_data_count();

		for (int j = 0; j < result_count; j++)
		{
			json_handler.append_to(obj, result[j].name, result[j].data);
		}
	}	

	return obj;

	return json_handler.create_object("test", 1);
}

// Fetches instruction form instruction queue
Json_container<JsonObject>* Input_handler::get_instruction()
{	
	return json_handler.fetch_instruction();
}

// Returns a list of available sensors
LinkedList<String, Sensor*>& Input_handler::get_sensor_collection()
{
	return sensor_collection;
}

const String & Input_handler::get_newline_format() const
{
	return newline_format;
}

bool Input_handler::instruction_available() const
{
	return !json_handler.queue_is_empty();
}
