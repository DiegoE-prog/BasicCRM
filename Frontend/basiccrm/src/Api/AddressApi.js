import axios from "axios"

const getAdressesAsync = async () => {
	const response = await axios.get("https://localhost:7081/api/Address/")
	return response
}

const getAddressAsync = async (id) => {
	const response = await axios.get(`https://localhost:7081/api/Address/${id}`)
	return response
}

const addAddressAsync = async (address) => {
	const response = await axios.post("https://localhost:7081/api/Address/", address)
	return response
}

const editAddressAsync = async (address) => {
	const response = await axios.put("https://localhost:7081/api/Address/", address)
	return response
}

const deleteAddressAsync = async (id) => {
	const response = await axios.delete(`https://localhost:7081/api/Address/${id}`)
	return response
}

export { getAdressesAsync, getAddressAsync, addAddressAsync, editAddressAsync, deleteAddressAsync }
