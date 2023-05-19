import React from "react"
import { useParams } from "react-router-dom"
import { useEffect, useState } from "react"
import { getAddressAsync, editAddressAsync } from "../../Api/AddressApi"
import { useNavigate } from "react-router-dom"
import Swal from "sweetalert2"
import withReactContent from "sweetalert2-react-content"
import FromAddress from "./FormAddress"

function EditAddress() {
	const { id } = useParams()
	const [address, setAddress] = useState({
		addressID: id,
		addressLine: "",
		addressDetails: "",
		city: "",
		state: "",
		zipCode: "",
		country: ""
	})

	const navigate = useNavigate()
	const MySwal = withReactContent(Swal)

	useEffect(() => {
		const fetchAddress = async () => {
			const responseFromApi = await getAddressAsync(id)
			const address = responseFromApi.data.content
			setAddress(address)
		}
		fetchAddress()
	}, [])

	const handleChange = (e) => {
		const value = e.target.value
		setAddress({
			...address,
			[e.target.name]: value
		})
	}

	const onSubmit = async () => {
		const addressData = {
			addressID: address.addressID,
			addressLine: address.addressLine,
			addressDetails: address.addressDetails,
			city: address.city,
			state: address.state,
			zipCode: address.zipCode,
			country: address.country
		}

		const response = await editAddressAsync(addressData)

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
			<div className="col-12 h4">Edit Address</div>
			<FromAddress onSubmit={onSubmit} address={address} setAddress={setAddress} />
		</div>
	)
}

export default EditAddress
