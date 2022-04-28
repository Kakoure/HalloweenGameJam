#pragma once

#include <vector>

class __declspec(dllexport)	
chessstate
{
	uint32_t width;
	uint32_t height;
	uint64_t* arr;

	chessstate(uint32_t width, uint32_t height) : arr(new uint64_t[(uint64_t)width * (uint64_t)height]), width(width), height(height) {}
};