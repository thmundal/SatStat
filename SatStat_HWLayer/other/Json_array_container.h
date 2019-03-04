#pragma once
#include "Arduino.h"
#include "Json_container.h"

class Json_array_container : public Json_container<JsonArray>
{
public:
	Json_array_container();

	void create();
	void parse(const String &json);
};