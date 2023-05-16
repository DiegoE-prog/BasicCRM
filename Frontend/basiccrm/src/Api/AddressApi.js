import axios from "axios"

const getAdressesAsync = async () => {
	const response = await axios.get("https://localhost:7081/api/Address/")
	return response
}

const getAddressAsync = async (id) => {
	const response = await axios.get(`https://localhost:7081/api/Address/${id}`)
	return response
}

export { getAdressesAsync, getAddressAsync }
