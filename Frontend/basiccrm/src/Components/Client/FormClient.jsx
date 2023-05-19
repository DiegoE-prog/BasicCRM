import { getAdressesAsync } from "../../Api/AddressApi"
import Select from "react-select"
import { useState, useEffect } from "react"
import { useForm } from "react-hook-form"

function FormClient(props) {
	const client = props.client
	const setClient = props.setClient
	const onSubmit = props.onSubmit
	const [addressOptions, setAddressOptions] = useState([])
	const [loading, setLoading] = useState(false)
	const [select, setSelect] = useState({})

	const {
		handleSubmit,
		register,
		reset,
		formState: { errors }
	} = useForm({
		defaultValues: {
			firstName: client.firstName,
			lastName: client.lastName,
			dateOfBirth: client.dateOfBirth,
			email: client.email,
			phoneNumber: client.phoneNumber,
			addressID: client.addressID
		}
	})

	useEffect(() => {
		reset(client)
	}, [client, reset])

	const fetchSelectData = async () => {
		setLoading(true)
		const response = await getAdressesAsync()
		const addresses = await response.data.content
		const addressOptions = addresses.map((address) => {
			return { value: address.addressID, label: address.addressLine }
		})
		setAddressOptions(addressOptions)
		setLoading(false)
	}

	const fetchSelect = (client) => {
		if (client.addressID !== null) {
			setSelect({ value: client.addressID, label: client.address?.addressLine })
		}
	}

	useEffect(() => {
		fetchSelectData()
		fetchSelect(client)
	}, [client])

	const handleChange = (e) => {
		const value = e.target.value
		setClient({
			...client,
			[e.target.name]: value
		})
	}

	return (
		<form onSubmit={handleSubmit(onSubmit)} className="row p-2">
			<div className="col-12 mb-2 p-1">
				<label htmlFor="firstName" className="form-label">
					First Name
				</label>
				<input
					type="text"
					name="firstName"
					{...register("firstName", {
						required: "First Name is Required"
					})}
					className="form-control form-control-sm"
					value={client.firstName}
					onChange={handleChange}
				/>
				<span className="text-danger">{errors.firstName && errors.firstName.message}</span>
			</div>
			<div className="col-12 mb-2 p-1">
				<label htmlFor="lastName" className="form-label">
					Last Name
				</label>
				<input
					type="text"
					name="lastName"
					{...register("lastName", {
						required: "Last Name is Required"
					})}
					className="form-control form-control-sm"
					value={client.lastName}
					onChange={handleChange}
				/>
				<span className="text-danger">{errors.lastName && errors.lastName.message}</span>
			</div>
			<div className="col-12 col-md-4 mb-2 p-1">
				<label htmlFor="dateOfBirth" className="form-label">
					Date of Birth
				</label>
				<input
					type="text"
					name="dateOfBirth"
					{...register("dateOfBirth", {
						required: "Date of Birth is required"
					})}
					className="form-control form-control-sm"
					value={client.dateOfBirth}
					onChange={handleChange}
				/>
				<span className="text-danger">{errors.dateOfBirth && errors.dateOfBirth.message}</span>
			</div>
			<div className="col-12 col-md-4 mb-2 p-1">
				<label htmlFor="email" className="form-label">
					Email
				</label>
				<input
					type="email"
					name="email"
					{...register("email", {
						required: "Email is Required",
						pattern: {
							value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
							message: "Please Enter A Valid Email!"
						}
					})}
					className="form-control form-control-sm"
					value={client.email}
					onChange={handleChange}
				/>
				<span className="text-danger">{errors.email && errors.email.message}</span>
			</div>
			<div className="col-12 col-md-4 mb-2 p-1">
				<label htmlFor="phoneNumber" className="form-label">
					Phone Number
				</label>
				<input
					type="text"
					name="phoneNumber"
					className="form-control form-control-sm"
					{...register("phoneNumber", {
						required: "Phone Number is Required",
						pattern: {
							value: /^[+]?[(]?[0-9]{3}[)]?[-\s.]?[0-9]{3}[-\s.]?[0-9]{4,6}$/i,
							message: "Please enter a valid Phone Number"
						}
					})}
					value={client.phoneNumber}
					onChange={handleChange}
				/>
				<span className="text-danger">{errors.phoneNumber && errors.phoneNumber.message}</span>
			</div>
			<div className="col-12 mb-2 p-1">
				<label htmlFor="addressID" className="form-label">
					Address
				</label>
				<Select
					options={addressOptions}
					isLoading={loading}
					name="addressID"
					value={select}
					onChange={(e) => {
						setSelect({ value: e.value, label: e.label })
						setClient({ ...client, addressID: e.value })
					}}
				/>
			</div>
			<div className="col-12 col-md-6 offset-md-3 p-2">
				<button type="submit" className="btn btn-primary btn-sm form-control">
					Edit
				</button>
			</div>
		</form>
	)
}

export default FormClient
