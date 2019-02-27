#pragma once
#include "Output_handler.h"

// Constructor
Output_handler::Output_handler()
{
	stepper = new Stepper(stepsPerRev, 8, 10, 9, 11);
	steps = 0;
	dir = false;
	auto_rotate_en = false;
	stepper->setSpeed(700);
	newline_format = "\n";
}

//Sends ack to software layer
void Output_handler::send_ack(LinkedList<String, Sensor*>& sensor_collection)
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

void Output_handler::send_ack()
{
	Json_container<JsonObject>* ack = json_handler.create_object("serial_connect", "ok");

	ack->get()->printTo(Serial);
	Serial.print(newline_format);

	delete ack;
}

void Output_handler::send_nack()
{
	Json_container<JsonObject>* nack = json_handler.create_object("serial_handshake", "failed");

	nack->get()->printTo(Serial);
	Serial.print(newline_format);

	delete nack;
}

void Output_handler::set_newline_format(const String & newline_format)
{
	this->newline_format = newline_format;
}

void Output_handler::print_to_serial(Json_container<JsonObject>* json)
{
	json->get()->printTo(Serial);
	Serial.print(newline_format);
	
	delete json;
}

/*
	Automatically rotates the SADM.
	Must be continuously called.
*/
void Output_handler::auto_rotate_sadm() 
{
	if (steps < step_limit)
	{
		steps++;
	}
	else
	{
		dir = !dir;
		steps = 0;
	}

	if (!dir)
	{
		stepper->step(1);
	}
	else
	{
		stepper->step(-1);
	}
}

bool Output_handler::auto_rotate_on()
{
	return auto_rotate_en;
}

// Rotates the SADM the passed number of steps
void Output_handler::rotate_sadm(int steps)
{
	stepper->step(steps);
}

// Converts from degrees to steps, and rotates the SADM
void Output_handler::rotate_sadm(float degrees)
{
	// 1 deg = 2048steps/360deg = 5.69 step/deg
	stepper->step((int)(degrees * ((float)2048 / (float)360)));
}
