using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OCG.ResourceManagement.Control.HeritanceControl;

namespace OCG.ResourceManagement.Control
{
    public class ResourceManager
    {
        private PersonControl _personControl;
        private GroupControl _groupControl;
        private OcgSSoDControl _ocgSSoDControl;
        private OcgOrgUnitControl _ocgOrgUnitControl;
        private OcgOrgAssignmentControl _ocgOrgAssignmentControl;
        private OcgRoleControl _ocgRoleControl;
        private ApprovalControl _approvalControl;
        private GenericControl _genericControl;

        public ClientControl ClientControl { get; set; }

        public ErrorControl ErrorControl
        {
            get
            {
                return ClientControl.ErrorControl;
            }
        }

        public ApprovalControl ApprovalControl
        {
            get { return _approvalControl; }
        }

        public PersonControl PersonControl
        {
            get { return _personControl; }
        }

        public GroupControl GroupControl
        {
            get { return _groupControl; }
        }

        public OcgSSoDControl OcgSSoDControl
        {
            get { return _ocgSSoDControl; }
        }

        public OcgOrgUnitControl OcgOrgUnitControl
        {
            get { return _ocgOrgUnitControl; }
        }

        public OcgOrgAssignmentControl OcgOrgAssignmentControl
        {
            get { return _ocgOrgAssignmentControl; }
        }

        public OcgRoleControl OcgRoleControl
        {
            get { return _ocgRoleControl; }
        }

        public GenericControl GenericControl
        {
            get { return _genericControl; }
        }

        public ResourceManager(ClientControl clientControl)
        {
            ClientControl = clientControl;

            _personControl = new PersonControl(ClientControl);
            _groupControl = new GroupControl(ClientControl);
            _ocgSSoDControl = new OcgSSoDControl(clientControl);
            _ocgOrgUnitControl = new OcgOrgUnitControl(clientControl);
            _ocgOrgAssignmentControl = new OcgOrgAssignmentControl(clientControl);
            _ocgRoleControl = new OcgRoleControl(clientControl);
            _approvalControl = new ApprovalControl(clientControl);
            _genericControl = new GenericControl(clientControl);
        }
    }
}
