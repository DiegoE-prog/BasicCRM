import React from "react"
import { useState } from "react"
import { addAddressAsync } from "../../Api/AddressApi"
import { useNavigate } from "react-router-dom"
import { useForm } from "react-hook-form"
import Swal from "sweetalert2"
import withReactContent from "sweetalert2-react-content"

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
	const MySwal = withReactContent(Swal)
	const {
		handleSubmit,
		register,
		formState: { errors }
	} = useForm()

	const handleChange = (e) => {
		console.log(errors)
		const value = e.target.value
		setAddress({
			...address,
			[e.target.name]: value
		})
	}

	const OnSubmit = async () => {
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
			<form onSubmit={handleSubmit(OnSubmit)}>
				<div className="col-12 mb-2 p-1">
					<label htmlFor="addressLine" class="form-label">
						Address Line
					</label>
					<input
						type="text"
						name="addressLine"
						{...register("addressLine", {
							required: "Required"
						})}
						className="form-control form-control-sm"
						value={address.addressLine}
						onChange={handleChange}
					/>
					<span className="text-danger">{errors.addressLine && errors.addressLine.message}</span>
				</div>
				<div className="col-12 mb-2  p-1">
					<label htmlFor="addressDetails" class="form-label">
						Address Details
					</label>
					<input type="text" name="addressDetails" className="form-control form-control-sm" value={address.addressDetails} onChange={handleChange} />
				</div>
				<div className="col-12 mb-2 p-1">
					<label htmlFor="city" class="form-label">
						City
					</label>
					<input
						type="text"
						name="city"
						{...register("city", {
							required: "Required"
						})}
						className="form-control form-control-sm"
						value={address.city}
						onChange={handleChange}
					/>
					<span className="text-danger">{errors.city && errors.city.message}</span>
				</div>
				<div className="col-12 mb-2 p-1">
					<label htmlFor="state" class="form-label">
						State
					</label>
					<input
						type="text"
						name="state"
						{...register("state", {
							required: "Required"
						})}
						className="form-control form-control-sm"
						value={address.state}
						onChange={handleChange}
					/>
					<span className="text-danger">{errors.state && errors.state.message}</span>
				</div>
				<div className="col-12 mb-2 p-1">
					<label htmlFor="zipCode" class="form-label">
						Zip Code
					</label>
					<input
						type="number"
						name="zipCode"
						{...register("zipCode", {
							required: "Zip Code is Required",
							pattern: {
								value: /^[0-9]{5}$/i,
								message: "Zip code needs to be a 5 digit number"
							}
						})}
						className="form-control form-control-sm"
						value={address.zipCode}
						onChange={handleChange}
					/>
					<span className="text-danger">{errors.zipCode && errors.zipCode?.message}</span>
				</div>
				<div className="col-12 mb-2 p-1">
					<label htmlFor="country" class="form-label">
						Country
					</label>
					<input
						type="text"
						name="country"
						{...register("country", {
							required: "Required"
						})}
						className="form-control form-control-sm"
						onChange={handleChange}
						value={address.country}
					/>
					<span className="text-danger">{errors.country && errors.country.message}</span>
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
