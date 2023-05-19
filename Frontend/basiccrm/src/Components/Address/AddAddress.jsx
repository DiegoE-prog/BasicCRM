import React from "react"
import { useState } from "react"
import { addAddressAsync } from "../../Api/AddressApi"
import { useNavigate } from "react-router-dom"

function AddAddress() {
	const [address, setAddress] = useState({
		addressLine: "",
		addressDetails: "",
		city: "",
		state: "",
		zipCode: "",
		country: ""
	})

	const navigate = useNavigate()

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
			addressLine: address.addressLine,
			addressDetails: address.addressDetails,
			city: address.city,
			state: address.state,
			zipCode: address.zipCode,
			country: address.country
		}

		const response = await addAddressAsync(addressData)
		response.then((response) => {
			if (response.data.success) {
				navigate("/Address/AddressIndex")
			}
		})
	}

	return (
		<div className="row p-2">
			<div className="col-12 h4">Add new Address</div>
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
						Add
					</button>
				</div>
			</form>
		</div>
	)
}

export default AddAddress
