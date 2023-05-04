import React from 'react'
import Navbar from '../../components/Navbar'
import ProfileSidebar from '../userCMS/ProfileSidebar'

function About() {
    return (
        <div className='container relative mx-auto'>
          <Navbar />
            {/* UserCMS-sidebar */}
            <div className='py-5 flex flex-row'>
            {/* Empty Sidebar */}
            <div className="flex flex-col w-1/5 text-left space-y-12 pt-10">
            </div>
                <div className="flex flex-col p-10 text-left justify-between text-lg leading-8 space-y-8">
                    <div>
                        Lorem ipsum dolor sit amet consectetur adipisicing elit. Rerum nihil eveniet quo eos sequi quibusdam cupiditate distinctio nemo porro aliquam amet eum, commodi itaque similique laborum odit non aut beatae eligendi possimus fugit? Quo earum reiciendis quasi quis ullam, autem eveniet obcaecati officia, voluptatum, cumque facere adipisci perspiciatis tenetur. Accusantium!
                    </div>
                    <div>
                        Lorem ipsum dolor sit amet consectetur adipisicing elit. Doloribus reiciendis incidunt voluptatum quidem, iure excepturi ad magni unde. Expedita, harum iusto in ratione accusantium tempora explicabo numquam molestiae a illum saepe provident ad perspiciatis consectetur. Quasi iure quisquam modi excepturi, numquam similique quam error nostrum alias nam ad distinctio odit?
                    </div>
                </div>
            </div>
        </div>
    )
}

export default About