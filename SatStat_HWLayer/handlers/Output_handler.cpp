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
	newline_format = "\r\n";	
	instruction_interpreter.append("auto_rotate", &Output_handler::set_auto_rotate);
	instruction_interpreter.append("rotate", &Output_handler::rotate_sadm);
}

void Output_handler::send_handshake_response()
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
void Output_handler::send_sensor_collection(LinkedList<String, Sensor*>& sensor_collection)
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
	Json_container<JsonObject>* ack = json_handler.create_object("connect", "init");
	
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

void Output_handler::set_auto_rotate(Json_container<JsonObject>* instruction)
{	
	auto_rotate_en = instruction->get()->get<bool>("enable");
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

void Output_handler::rotate_sadm(Json_container<JsonObject>* instruction)
{
	if (instruction->get()->containsKey("deg"))
	{
		float deg = instruction->get()->get<float>("deg");
		rotate_sadm(deg);
	}
	else if (instruction->get()->containsKey("steps"))
	{
		int steps = instruction->get()->get<int>("steps");
		rotate_sadm(steps);
	}
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

void Output_handler::interpret_instruction(Json_container<JsonObject>* obj)
{
	String instruction = obj->get()->get<String>("instruction");
	void(Output_handler::*ptr)(Json_container<JsonObject>*);
	ptr = instruction_interpreter.get(instruction);
	(*this.*ptr)(obj);
	delete obj;
}

bool Output_handler::get_auto_rotate_en() const
{
	return auto_rotate_en;
}
