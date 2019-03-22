#pragma once
#include "Json_container.h"

/**
*	Child class of Json_container instantiated as Json_container<JsonObject>.
*	Overrides the create and parse methods, and has a constructor for instantiating the json member variable in the parent class as a JsonObject type.
*/
class Json_object_container : public Json_container<JsonObject>
{
public:
	Json_object_container();

	void create();
	bool parse(const String &json);
};