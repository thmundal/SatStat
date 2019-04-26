#pragma once
#include "Sensor_container.h"

/**
*	Instantiate sensors with name and the pin it's connected to.
*/
Sensor_container::Sensor_container()
{
	sensor_collection.append("temp_hum", new Temp_hum_sensor("temp_hum", 4));
}

/**
*	Delete every sensor in the sensor collection.
*/
Sensor_container::~Sensor_container()
{
	for (int i = 0; i < sensor_collection.count(); i++)
	{
		delete sensor_collection[i];
	}
}

/**
*	Read sensor corresponding to the name sent as argument, and return result as Json_container<JsonObject> pointer.
*/
void Sensor_container::read_sensor(const String& name)
{
	Sensor* sensor = sensor_collection.get(name);	
	sensor->read_sensor();	
}

/**
*	Reads all sensors in the sensor collection, and return result as a Json_container<JsonObject> pointer.
*/
void Sensor_container::read_all_sensors()
{	
	for (int i = 0; i < sensor_collection.count(); i++)
	{	
		read_sensor(sensor_collection[i]->get_name());
	}
}

void Sensor_container::append_data(Json_container<JsonObject>* dest, sstl::Subscribable* src)
{
	sstl::types type = src->get_type();

	if (type == sstl::types::t_int)
	{
		auto data_obj = sstl::downcast<int>(src);
		data_obj->get_name();
		json_handler.append_to(dest, data_obj->get_name(), data_obj->get_data());
	}
	else if (type == sstl::types::t_float)
	{
		auto data_obj = sstl::downcast<float>(src);
		data_obj->get_name();
		json_handler.append_to(dest, data_obj->get_name(), data_obj->get_data());
	}
	else if (type == sstl::types::t_double)
	{
		auto data_obj = sstl::downcast<double>(src);
		data_obj->get_name();
		json_handler.append_to(dest, data_obj->get_name(), data_obj->get_data());
	}
}

Json_container<JsonObject>* Sensor_container::get_data(const String& name)
{
	Json_container<JsonObject>* obj = json_handler.create_object();
	auto sub_obj = sstl::Lists::get_data(name);		

	append_data(obj, sub_obj);
	return obj;
}

Json_container<JsonObject>* Sensor_container::get_all_data()
{
	auto& list = sstl::Lists::get_data_list();
	int count = list.count();

	if (count != 0)
	{
		Json_container<JsonObject>* obj = json_handler.create_object();

		for (int i = 0; i < count; i++)
		{
			append_data(obj, list[i]);
		}

		return obj;
	}
	
	return nullptr;
}

Json_container<JsonObject>* Sensor_container::get_sub_data()
{
	auto& list = sstl::Lists::get_sub_list();
	int count = list.count();

	if (count != 0)
	{
		Json_container<JsonObject>* obj = json_handler.create_object();

		for (int i = 0; i < count; i++)
		{
			append_data(obj, list[i]);
		}

		return obj;
	}

	return nullptr;
}

void Sensor_container::append_available_data(Json_container<JsonObject>* dest)
{
	JsonObject& tmp = dest->get().createNestedObject("available_data");

	auto& data_list = sstl::Lists::get_data_list();

	for (int i = 0; i < data_list.count(); i++)
	{
		auto data = data_list[i];
		String type;

		switch (data_list[i]->get_type())
		{
		case sstl::types::t_int:
			type = "int";
			break;
		case sstl::types::t_float:
			type = "float";
			break;
		case sstl::types::t_double:
			type = "double";
			break;
		}

		tmp.set(data_list[i]->get_name(), type);
	}
}

/**
*	Returns a reference to the linked list containing every available sensor.
*/
LinkedList<String, Sensor*>& Sensor_container::get_available_sensors()
{
	return sensor_collection;
}
