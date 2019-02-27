#pragma once
#include "Arduino.h"
#include "Json_handler.h"
#include "../libraries/LinkedList.h"
#include "../sensors/Temp_hum_sensor.h"

class Input_handler
{
public:
	// Constructor and destructor	
	Input_handler();
	~Input_handler();

	// Serial methods
	bool establish_connection();
	void serial_listener();
	void serial_init();
	bool init_connection();

	// Sensor methods
	Json_container<JsonObject>* read_sensor(const String& name);
	Json_container<JsonObject>* read_sensors();

	// Getters
	Json_container<JsonObject>* get_instruction();
	LinkedList<String, Sensor*>& get_sensor_collection();
	const String& get_newline_format() const;

private:		
	bool config_approved(const unsigned long& baud_rate, const String& config);	

	Json_handler json_handler;
	LinkedList<String, Sensor*> sensor_collection;

	unsigned long baud_rate;
	String config;
	String newline_format;
};
