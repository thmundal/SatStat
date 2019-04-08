#include "./libraries/SSTL/sstl.h"

// the setup function runs once when you press reset or power the board
void setup() {
	Serial.begin(9600);

	auto temp = new sstl::Data_container<int>("temperature");
	auto hum = new sstl::Data_container<float>("humidity");
	auto some = new sstl::Data_container<double>("something");
	
	temp->get_data().val = 10;
	hum->get_data().val = 2.2f;
	some->get_data().val = 1.23;

	sstl::Lists::append_data("temperature", temp);
	sstl::Lists::append_data("humidity", hum);
	sstl::Lists::append_data("something", some);
	

	sstl::Lists::subscribe("temperature");
	sstl::Lists::subscribe("humidity");
	sstl::Lists::subscribe("something");	
}

// the loop function runs over and over again until power down or reset
void loop() {
	auto& sub_list = sstl::Lists::get_sub_list();

	sstl::types type;

	for (int i = 0; i < sub_list.count(); i++)
	{
		//Serial.println(i);
		auto sub = sub_list[i];
		type = sub->get_type();

		if (type == sstl::types::t_int)
		{
			sstl::Data_container<int>* data = (sstl::Data_container<int>*)sub;
			Serial.println("int");
		}
		else if (type == sstl::types::t_float)
		{
			sstl::Data_container<float>* data = (sstl::Data_container<float>*)sub;
			Serial.println("float");
		}
		else if (type == sstl::types::t_double)
		{
			sstl::Data_container<double>* data = (sstl::Data_container<double>*)sub;
			Serial.println("double");
		}
	}

	delay(1000);
}
