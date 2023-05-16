import { NavLink } from "react-router-dom"

function Header() {
	return (
		<header>
			<div className="pt-3 py-1 pl-2 border-bottom">
				<ul className="nav col-12 col-lg-auto my-2 my-md-0">
					<li>
						<NavLink to="/" className="nav-link text-dark">
							<i class="bi bi-house-door"></i> Home
						</NavLink>
					</li>
					<li>
						<NavLink to="" className="nav-link text-dark">
							<i class="bi bi-people-fill"></i> Clients
						</NavLink>
					</li>
					<li>
						<NavLink to="/Address/AddressIndex" className="nav-link text-dark">
							<i class="bi bi-globe"></i> Addresses
						</NavLink>
					</li>
				</ul>
			</div>
		</header>
	)
}

export default Header
