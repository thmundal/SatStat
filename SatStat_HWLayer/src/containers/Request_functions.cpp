#pragma once
#include "Request_functions.h"

void Request_functions::subscribe(Json_container<JsonObject>& request)
{
	if (request->containsKey("parameters"))
	{
		if (request->is<JsonArray>("parameters"))
		{
			JsonArray& params = request->get<JsonArray>("parameters");
			String str;

			for (size_t i = 0; i < params.measureLength(); i++)
			{
				str = params[i].asString();
				sstl::Lists::subscribe(str);
			}			
		}
		else
		{
			Json_container<JsonObject> tmp;
			tmp->set("error", "Invalid parameters!");
			tmp->printTo(Serial);
		}
	}
	else
	{
		Json_container<JsonObject> tmp;
		tmp->set("error", "Invalid argument!");
		tmp->printTo(Serial);
	}
}

void Request_functions::unsubscribe(Json_container<JsonObject>& request)
{
	if (request->containsKey("parameters"))
	{
		if (request->is<JsonArray>("parameters"))
		{
			JsonArray& params = request->get<JsonArray>("parameters");
			String str;

			for (size_t i = 0; i < params.measureLength(); i++)
			{
				str = params[i].asString();
				sstl::Lists::unsubscribe(str);
			}
		}
		else
		{
			Json_container<JsonObject> tmp;
			tmp->set("error", "Invalid parameters!");
			tmp->printTo(Serial);
		}
	}
	else
	{
		Json_container<JsonObject> tmp;
		tmp->set("error", "Invalid argument!");
		tmp->printTo(Serial);
	}
}

void Request_functions::reset(Json_container<JsonObject>& request)
{
	void(*reset)() = 0;
	reset();
}
