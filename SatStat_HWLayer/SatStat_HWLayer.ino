#pragma once
#include "./srcs/sensors/Temp_hum_sensor.h"

Temp_hum_sensor* temp_hum;
//LinkedList<String, sstl::Subscribable*>* list;

void setup()
{	
	Serial.begin(9600);

	pinMode(5, OUTPUT);
	pinMode(6, INPUT);
	pinMode(7, OUTPUT);

	digitalWrite(5, HIGH);
	digitalWrite(7, LOW);

	temp_hum = new Temp_hum_sensor("temp_hum", 6);
	sstl::Lists::subscribe("temperature");
	sstl::Lists::subscribe("humidity");		
	
	//list = &sstl::Lists::get_sub_list();
	Serial.println("test");
	
}

void loop()
{			
	temp_hum->read_sensor();		

	sstl::Subscribable* sub = sstl::Lists::get_data("temperature");

	auto data = sstl::downcast<double>(sub);

	Serial.println(data->get_data());

	delay(500);

	/*for (size_t i = 0; i < list.count(); i++)
	{
		if (type == sstl::types::t_float)
		{
			auto data = sstl::downcast<float>(sub);
			Serial.print(data->get_name() + ": ");
			Serial.println(data->get_data());

		}
		else if (type == sstl::types::t_double)
		{
			auto data = sstl::downcast<double>(sub);
			Serial.print(data->get_name() + ": ");
			Serial.println(data->get_data());
		}
		else
		{
			auto data = sstl::downcast<int>(sub);
			Serial.print(data->get_name() + ": ");
			Serial.println(data->get_data());
		}
	}*/
}