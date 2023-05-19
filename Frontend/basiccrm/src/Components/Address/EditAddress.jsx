import React from "react"
import { useParams } from "react-router-dom"
import { useEffect, useState } from "react"
import { getAddressAsync, editAddressAsync } from "../../Api/AddressApi"
import { useNavigate } from "react-router-dom"
import Swal from "sweetalert2"
import withReactContent from "sweetalert2-react-content"

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

	const handleSubmit = async (e) => {
		e.preventDefault()
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
			<form onSubmit={handleSubmit}>
				<div className="col-12 mb-2 p-1">
					<label htmlFor="addressLine" class="form-label">
						Address Line
					</label>
					<input type="text" name="addressLine" className="form-control form-control-sm" value={address.addressLine} onChange={handleChange} />
				</div>
				<div className="col-12 mb-2  p-1">
					<label htmlFor="addressDetails" class="form-label">
						Address Line
					</label>
					<input type="text" name="addressDetails" className="form-control form-control-sm" value={address.addressDetails} onChange={handleChange} />
				</div>
				<div className="col-12 mb-2 p-1">
					<label htmlFor="city" class="form-label">
						City
					</label>
					<input type="text" name="city" className="form-control form-control-sm" value={address.city} onChange={handleChange} />
				</div>
				<div className="col-12 mb-2 p-1">
					<label htmlFor="state" class="form-label">
						State
					</label>
					<input type="text" name="state" className="form-control form-control-sm" value={address.state} onChange={handleChange} />
				</div>
				<div className="col-12 mb-2 p-1">
					<label htmlFor="zipCode" class="form-label">
						Zip Code
					</label>
					<input type="number" name="zipCode" className="form-control form-control-sm" value={address.zipCode} onChange={handleChange} />
				</div>
				<div className="col-12 mb-2 p-1">
					<label htmlFor="country" class="form-label">
						Country
					</label>
					<input type="text" name="country" className="form-control form-control-sm" value={address.country} onChange={handleChange} />
				</div>
				<div className="col-12 col-md-6 offset-md-3 p-2">
					<button type="submit" className="btn btn-primary btn-sm form-control">
						Edit
					</button>
				</div>
			</form>
		</div>
	)
}

export default EditAddress
