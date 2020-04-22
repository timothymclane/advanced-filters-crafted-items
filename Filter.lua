local util = AdvancedFilters.util
local function CraftedFilter(isCrafted)
	return function (slot, slotIndex)
		if util.prepareSlot ~= nil then
			if slotIndex ~= nil and type(slot) ~= "table" then
				slot = util.prepareSlot(slot, slotIndex)
			end
		end

		local itemLink = GetItemLink(slot.bagId, slot.slotIndex)
		if itemLink == nil then
			return false
		end
		
		return IsItemLinkCrafted(itemLink) == isCrafted
	end
end

local CraftedFilters = {
	{ name = "Crafted", filterCallback = CraftedFilter(true) },
	{ name = "NotCrafted", filterCallback = CraftedFilter(false) },
}

local en = {
	["Crafted"] = "Crafted",
	["NotCrafted"] = "Not crafted",
}

local filterInformation = {
	callbackTable = CraftedFilters,
	filterType = ITEMFILTERTYPE_ALL,
	subfilters = { "All" },
	enStrings = en,
}

AdvancedFilters_RegisterFilter(filterInformation)