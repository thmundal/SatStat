#pragma once
#include "Arduino.h"
#include "Json_handler.h"
#include "../libraries/Stepper.h"
#include "../libraries/DS1302.h"
#include "../libraries/LinkedList.h"
#include "../sensors/Sensor.h"
#include "../other/Json_object_container.h"
#include "../other/Json_array_container.h"

class Output_handler
{
public:
	Output_handler();

	void send_handshake_response();
	void send_sensor_collection(LinkedList<String, Sensor*>& sensor_collection);
	void send_ack();
	void send_nack();

	void set_newline_format(const String& newline_format);

	void print_to_serial(Json_container<JsonObject>* json);	

	void set_auto_rotate(Json_container<JsonObject>* instruction);
	void auto_rotate_sadm();
	void rotate_sadm(Json_container<JsonObject>* instruction);
	void rotate_sadm(int steps);
	void rotate_sadm(float degrees);

	void interpret_instruction(Json_container<JsonObject>* obj);
	
	bool get_auto_rotate_en() const;

private:
	Json_handler json_handler;	
	Stepper* stepper;
	int steps;
	bool dir;
	bool auto_rotate_en;
	const int stepsPerRev = 32;
	const float factor = 3.25;
	const int step_limit = (int)(1024 * factor);
	
	String newline_format;			

	LinkedList<String, void(Output_handler::*)(Json_container<JsonObject>*)> instruction_interpreter;
};

