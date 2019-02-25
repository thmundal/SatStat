#include "Json_array_container.h"

Json_array_container::Json_array_container() : Json_container()
{
	Json_container::json = &Json_container::buffer->createArray();
}

void Json_array_container::create()
{
	delete Json_container::buffer;
	Json_container::buffer = new DynamicJsonBuffer;

	Json_container::json = &Json_container::buffer->createArray();
}

void Json_array_container::parse(const String & json)
{
	delete Json_container::buffer;
	Json_container::buffer = new DynamicJsonBuffer;

	Json_container::json = &Json_container::buffer->parseArray(json);
}

JsonArray* Json_array_container::get()
{
	return Json_container::json;
}
