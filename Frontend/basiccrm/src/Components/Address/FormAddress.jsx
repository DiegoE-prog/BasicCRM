import { useForm } from "react-hook-form"
import { useEffect } from "react"

function FromAddress(props) {
	const onSubmit = props.onSubmit
	const address = props.address
	const setAddress = props.setAddress

	const {
		handleSubmit,
		register,
		reset,
		formState: { errors }
	} = useForm({
		defaultValues: {
			addressLine: address.addressLine,
			city: address.city,
			state: address.state,
			zipCode: address.zipCode,
			country: address.country
		}
	})

	useEffect(() => {
		reset(address)
	}, [address, reset])

	const handleChange = (e) => {
		const value = e.target.value
		setAddress({
			...address,
			[e.target.name]: value
		})
	}

	return (
		<form onSubmit={handleSubmit(onSubmit)}>
			<div className="col-12 mb-2 p-1">
				<label htmlFor="addressLine" class="form-label">
					Address Line
				</label>
				<input
					type="text"
					className="form-control form-control-sm"
					value={address.addressLine}
					name="addressLine"
					{...register("addressLine", {
						required: "Required"
					})}
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
					value={address.city}
					{...register("city", {
						required: "Required"
					})}
					className="form-control form-control-sm"
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
					className="form-control form-control-sm"
					value={address.state}
					{...register("state", {
						required: "Required"
					})}
					onChange={handleChange}
				/>
				<span className="text-danger">{errors.state && errors.state.message}</span>
			</div>
			<div className="col-12  mb-2 p-1">
				<label htmlFor="zipCode" class="form-label">
					Zip Code
				</label>
				<input
					type="number"
					name="zipCode"
					className="form-control form-control-sm"
					value={address.zipCode}
					{...register("zipCode", {
						required: "Zip Code is Required",
						pattern: {
							value: /^[0-9]{5}$/i,
							message: "Zip code needs to be a 5 digit number"
						}
					})}
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
					className="form-control form-control-sm"
					value={address.country}
					name="country"
					{...register("country", {
						required: "Required"
					})}
					onChange={handleChange}
				/>
				<span className="text-danger">{errors.country && errors.country.message}</span>
			</div>
			<div className="col-12 col-md-6 offset-md-3 p-2">
				<button type="submit" className="btn btn-primary btn-sm form-control">
					{address.addressID === "" ? "Add" : "Edit"}
				</button>
			</div>
		</form>
	)
}

export default FromAddress
