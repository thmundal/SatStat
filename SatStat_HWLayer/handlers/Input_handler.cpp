#pragma once
#include "Input_handler.h"

// Instantiate sensors and establish connection
Input_handler::Input_handler()
{	
	sensor_collection.append("temp_hum", new Temp_hum_sensor("temp_hum", 6));
	//while (!establish_connection());
	//send_ack();
}

// Delete every sensor in the sensor collection
Input_handler::~Input_handler()
{
	for (int i = 0; i < sensor_collection.count(); i++)
	{
		delete sensor_collection[i];
	}	
}

/*
	Reads serial and check if initial handshake is received.
	Init serial when handshake received.
	Send ack.
*/
bool Input_handler::establish_connection()
{
	serial_listener();
	if (!json_handler.queue_is_empty())
	{
		JsonObject* tmp = json_handler.fetch_instruction();
		JsonObject* nested_obj;

		if (tmp->containsKey("connect"))
		{
			nested_obj = &tmp->get<JsonObject>("connect");
			
			serial_init(nested_obj->get<unsigned long>("baud_rate"), nested_obj->get<String>("config"));

			newline_format = nested_obj->get<String>("newline");
		}

		delete &tmp;
		delete &nested_obj;
		
		send_ack();
	}

	return false;
}

// Init serial with given config
void Input_handler::serial_init(const unsigned long & baud_rate, const String& config)
{
	if (config == "SERIAL_5N1")		{Serial.begin(baud_rate, SERIAL_5N1);}
	else if (config == "SERIAL_6N1"){Serial.begin(baud_rate, SERIAL_6N1);}
	else if (config == "SERIAL_7N1"){Serial.begin(baud_rate, SERIAL_7N1);}
	else if (config == "SERIAL_8N1"){Serial.begin(baud_rate, SERIAL_8N1);}
	else if (config == "SERIAL_5N2"){Serial.begin(baud_rate, SERIAL_5N2);}
	else if (config == "SERIAL_6N2"){Serial.begin(baud_rate, SERIAL_6N2);}
	else if (config == "SERIAL_7N2"){Serial.begin(baud_rate, SERIAL_7N2);}
	else if (config == "SERIAL_8N2"){Serial.begin(baud_rate, SERIAL_8N2);}
	else if (config == "SERIAL_5E1"){Serial.begin(baud_rate, SERIAL_5E1);}
	else if (config == "SERIAL_6E1"){Serial.begin(baud_rate, SERIAL_6E1);}
	else if (config == "SERIAL_7E1"){Serial.begin(baud_rate, SERIAL_7E1);}
	else if (config == "SERIAL_8E1"){Serial.begin(baud_rate, SERIAL_8E1);}
	else if (config == "SERIAL_5E2"){Serial.begin(baud_rate, SERIAL_5E2);}
	else if (config == "SERIAL_6E2"){Serial.begin(baud_rate, SERIAL_6E2);}
	else if (config == "SERIAL_7E2"){Serial.begin(baud_rate, SERIAL_7E2);}
	else if (config == "SERIAL_8E2"){Serial.begin(baud_rate, SERIAL_8E2);}
	else if (config == "SERIAL_5O1"){Serial.begin(baud_rate, SERIAL_5O1);}
	else if (config == "SERIAL_6O1"){Serial.begin(baud_rate, SERIAL_6O1);}
	else if (config == "SERIAL_7O1"){Serial.begin(baud_rate, SERIAL_7O1);}
	else if (config == "SERIAL_8O1"){Serial.begin(baud_rate, SERIAL_8O1);}
	else if (config == "SERIAL_5O2"){Serial.begin(baud_rate, SERIAL_5O2);}
	else if (config == "SERIAL_6O2"){Serial.begin(baud_rate, SERIAL_6O2);}
	else if (config == "SERIAL_7O2"){Serial.begin(baud_rate, SERIAL_7O2);}
	else if (config == "SERIAL_8O2"){Serial.begin(baud_rate, SERIAL_8O2);}
}

// Sends ack to software layer
void Input_handler::send_ack()
{	
	JsonObject* ack = json_handler.to_json_object("connection", "ok");
	JsonArray& available_data = ack->createNestedArray("available_data");

	for (int i = 0; i < sensor_collection.count(); i++)
	{
		Sensor* sensor = sensor_collection[i];
		for (size_t i = 0; i < sensor->get_data_count(); i++)
		{			
			available_data.add(json_handler.to_json_object(sensor->read_sensor()[i].name, "int"));
		}
	}

	ack->printTo(Serial);
	Serial.print(newline_format);

	delete &ack;
}

// Listens on inputs from software layer
void Input_handler::serial_listener()
{
	if (Serial.available() > 0) 
	{
		String input = Serial.readStringUntil('\n');

		if (input)
		{
			json_handler.insert_instruction(input);
		}
		else
		{
			//Serial.println("No serial input received");
		}
	}
}

// Read given sensor
const JsonObject* Input_handler::read_sensor(const String& name)
{
	Sensor* sensor = sensor_collection.get(name);
	return json_handler.to_json_object(sensor->read_sensor(), sensor->get_data_count());
}

// Reads all sensors
const JsonObject* Input_handler::read_sensors()
{	
	const Result* result;

	for (int i = sensor_collection.count() - 1; i >= 0; i--)
	{
		result = sensor_collection[i]->read_sensor();

		for (int j = 0; j < sensor_collection[i]->get_data_count(); j++)
		{
			json_handler.to_json_object(result[i].name, result[i].data);
		}

		// delete result;
	}

	return json_handler.to_json_object("test", 1);
}

// Fetches instruction form instruction queue
JsonObject* Input_handler::get_instruction()
{
	return json_handler.fetch_instruction();
}

// Returns a list of available sensors
String Input_handler::get_sensor_list()
{
	String tmp = "{\"available_sensors\":[";
	for (int i = sensor_collection.count() - 1; i >= 0; i--)
	{
		 tmp += "\"" + sensor_collection[i]->get_name() + "\"";
		 
		 if (i != 0)
		 {
			 tmp += ",";
		 }
		 else
		 {
			 tmp += "]}";
		 }
	}
	return tmp;	
}

const String & Input_handler::get_newline_format() const
{
	return newline_format;
}
