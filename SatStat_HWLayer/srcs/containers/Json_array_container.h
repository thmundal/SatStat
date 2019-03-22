#pragma once
#include "Json_container.h"

/**
*	Child class of Json_container instantiated as Json_container<JsonArray>.
*	Overrides the create and parse methods, and has a constructor for instantiating the json member variable in the parent class as a JsonArray type.
*/
class Json_array_container : public Json_container<JsonArray>
{
public:
	Json_array_container();

	void create();
	bool parse(const String &json);
};