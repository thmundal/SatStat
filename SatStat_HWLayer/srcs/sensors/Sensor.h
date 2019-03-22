#pragma once
#include "Result.h"

/**
*	The Sensor class is the parent class of every specific sensor added to the system.
*	The purpose of this class is to ensure that every sensor are of the same type, to be able to store all of them in a collection.
*	This is an abstract class, and it foreces subclasses to override the read_sensor function as this is different for every sensor, but is yet required.
*	get_name and get_data_count are inherited for every child as they do exactly the same for every type of sensor.
*/
class Sensor
{
public:
	Sensor(const String& name, const int& pin, const int& data_count = 1);
	virtual ~Sensor();

	virtual const Result* read_sensor() = 0;
	const String& get_name() const;
	const int& get_data_count() const;

protected:
	String name;
	int pin;	
	int data_count;
	Result* result;	
};