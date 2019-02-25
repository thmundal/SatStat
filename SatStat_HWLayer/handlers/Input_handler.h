#pragma once
#include "Arduino.h"
#include "Json_handler.h"
#include "../libraries/LinkedList.h"
#include "../sensors/Temp_hum_sensor.h"

class Input_handler
{
public:
	Input_handler(); // Constructor	
	~Input_handler(); // Destructor	
	
	void serial_listener(); // Listens on inputs from software layer
	Json_container<JsonObject>* read_sensor(const String& name);
	Json_container<JsonObject>* read_sensors(); // Takes the sensor name as a string, and returns the read value as an integer	
	Json_container<JsonObject>* get_instruction();
	LinkedList<String, Sensor*>& get_sensor_collection();
	const String& get_newline_format() const;

private:
	bool establish_connection();
	void serial_init(const unsigned long& baud_rate, const String& config);	

	Json_handler json_handler;
	LinkedList<String, Sensor*> sensor_collection;
	String newline_format;
};
