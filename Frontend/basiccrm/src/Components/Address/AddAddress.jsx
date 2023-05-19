import React from "react"
import { useState } from "react"
import { addAddressAsync } from "../../Api/AddressApi"
import { useNavigate } from "react-router-dom"

import Swal from "sweetalert2"
import withReactContent from "sweetalert2-react-content"
import FromAddress from "./FormAddress"

function AddAddress() {
	const [address, setAddress] = useState({
		addressID: "",
		addressLine: "",
		addressDetails: "",
		city: "",
		state: "",
		zipCode: "",
		country: ""
	})

	const MySwal = withReactContent(Swal)
	const navigate = useNavigate()

	const onSubmit = async () => {
		const addressData = {
			addressLine: address.addressLine,
			addressDetails: address.addressDetails,
			city: address.city,
			state: address.state,
			zipCode: address.zipCode,
			country: address.country
		}

		const response = await addAddressAsync(addressData)
		console.log(response)
		if (response.data.success) {
			MySwal.fire({
				position: "top-end",
				icon: "success",
				title: response.data.message,
				showConfirmButton: false,
				timer: 2000
			})
			navigate("/Address/AddressIndex")
		}
	}

	return (
		<div className="row p-2">
			<div className="col-12 h4">Add new Address</div>
			<FromAddress onSubmit={onSubmit} address={address} setAddress={setAddress} />
		</div>
	)
}

export default AddAddress
