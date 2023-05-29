import axios from "axios"

const URL = process.env.NODE_ENV === "development" ? "https://localhost:7081" : "https://dev-crmbasic-wepapi.azurewebsites.net"

const getAdressesAsync = async () => {
	const response = await axios.get(`${URL}/api/Address/`)
	return response
}

const getAddressAsync = async (id) => {
	const response = await axios.get(`${URL}/api/Address/${id}`)
	return response
}

const addAddressAsync = async (address) => {
	const response = await axios.post(`${URL}/api/Address/`, address)
	return response
}

const editAddressAsync = async (address) => {
	const response = await axios.put(`${URL}/api/Address/`, address)
	return response
}

const deleteAddressAsync = async (id) => {
	const response = await axios.delete(`${URL}/api/Address/${id}`)
	return response
}

export { getAdressesAsync, getAddressAsync, addAddressAsync, editAddressAsync, deleteAddressAsync }
