DELETE FROM Departments;
DBCC CHECKIDENT (Departments, RESEED, 0)
INSERT INTO Departments(Name, created_at, created_by, updated_at, updated_by)
VALUES
(N'Phòng Nhân sự', GETDATE(), 0, GETDATE(), 0),
(N'Phòng Kinh doanh', GETDATE(), 0, GETDATE(), 0),
(N'Phòng Marketing', GETDATE(), 0, GETDATE(), 0),
(N'Phòng Tài chính', GETDATE(), 0, GETDATE(), 0),
(N'Phòng Kế toán', GETDATE(), 0, GETDATE(), 0),
(N'Phòng Hỗ trợ khách hàng', GETDATE(), 0, GETDATE(), 0),
(N'Phòng Công nghệ thông tin', GETDATE(), 0, GETDATE(), 0),
(N'Phòng Phát triển sản phẩm', GETDATE(), 0, GETDATE(), 0),
(N'Phòng Nghiên cứu và Phát triển', GETDATE(), 0, GETDATE(), 0),
(N'Phòng Quản lý dự án', GETDATE(), 0, GETDATE(), 0),
(N'Phòng Mua sắm', GETDATE(), 0, GETDATE(), 0),
(N'Phòng Logistic', GETDATE(), 0, GETDATE(), 0),
(N'Phòng Truyền thông', GETDATE(), 0, GETDATE(), 0),
(N'Phòng Đào tạo', GETDATE(), 0, GETDATE(), 0),
(N'Phòng Pháp lý', GETDATE(), 0, GETDATE(), 0),
(N'Phòng Kiểm soát chất lượng', GETDATE(), 0, GETDATE(), 0),
(N'Phòng Bán hàng', GETDATE(), 0, GETDATE(), 0),
(N'Phòng Thiết kế', GETDATE(), 0, GETDATE(), 0),
(N'Phòng Dịch vụ kỹ thuật', GETDATE(), 0, GETDATE(), 0),
(N'Phòng Đối ngoại', GETDATE(), 0, GETDATE(), 0);


DELETE FROM Positions;
DBCC CHECKIDENT (Positions, RESEED, 0)

INSERT INTO Positions(Name, DepartmentId, CurrentPositionsFilled, TotalPositionsNeeded, created_at, created_by, updated_at, updated_by)
VALUES
-- Phòng Nhân sự
(N'Giám đốc nhân sự', 1, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Chuyên viên tuyển dụng', 1, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Nhân viên đào tạo', 1, 5, 5, GETDATE(), 0, GETDATE(), 0),

-- Phòng Kinh doanh
(N'Giám đốc kinh doanh', 2, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Quản lý bán hàng', 2, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Nhân viên kinh doanh', 2, 5, 5, GETDATE(), 0, GETDATE(), 0),

-- Phòng Marketing
(N'Giám đốc marketing', 3, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Chuyên viên truyền thông', 3, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Nhân viên marketing số', 3, 5, 5, GETDATE(), 0, GETDATE(), 0),

-- Phòng Tài chính
(N'Trưởng phòng tài chính', 4, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Nhân viên kế toán', 4, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Chuyên viên phân tích tài chính', 4, 5, 5, GETDATE(), 0, GETDATE(), 0),

-- Phòng Kế toán
(N'Kế toán trưởng', 5, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Nhân viên kế toán tổng hợp', 5, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Nhân viên kế toán chi tiết', 5, 5, 5, GETDATE(), 0, GETDATE(), 0),

-- Phòng Hỗ trợ khách hàng
(N'Trưởng phòng hỗ trợ', 6, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Nhân viên hỗ trợ khách hàng', 6, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Chuyên viên chăm sóc khách hàng', 6, 5, 5, GETDATE(), 0, GETDATE(), 0),

-- Phòng Công nghệ thông tin
(N'Giám đốc công nghệ', 7, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Kỹ sư phần mềm', 7, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Nhân viên hỗ trợ IT', 7, 5, 5, GETDATE(), 0, GETDATE(), 0),

-- Phòng Phát triển sản phẩm
(N'Trưởng phòng phát triển sản phẩm', 8, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Chuyên viên phát triển sản phẩm', 8, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Nhân viên thiết kế sản phẩm', 8, 5, 5, GETDATE(), 0, GETDATE(), 0),

-- Phòng Nghiên cứu và Phát triển
(N'Trưởng phòng R&D', 9, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Nhà nghiên cứu', 9, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Kỹ thuật viên R&D', 9, 5, 5, GETDATE(), 0, GETDATE(), 0),

-- Phòng Quản lý dự án
(N'Quản lý dự án', 10, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Chuyên viên quản lý dự án', 10, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Trợ lý quản lý dự án', 10, 5, 5, GETDATE(), 0, GETDATE(), 0),

-- Phòng Mua sắm
(N'Trưởng phòng mua sắm', 11, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Nhân viên mua sắm', 11, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Chuyên viên phân tích mua sắm', 11, 5, 5, GETDATE(), 0, GETDATE(), 0),

-- Phòng Logistic
(N'Trưởng phòng logistic', 12, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Nhân viên logistic', 12, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Nhân viên quản lý kho', 12, 5, 5, GETDATE(), 0, GETDATE(), 0),

-- Phòng Truyền thông
(N'Trưởng phòng truyền thông', 13, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Chuyên viên truyền thông', 13, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Nhân viên truyền thông xã hội', 13, 5, 5, GETDATE(), 0, GETDATE(), 0),

-- Phòng Đào tạo
(N'Trưởng phòng đào tạo', 14, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Nhân viên đào tạo', 14, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Chuyên viên phát triển nhân lực', 14, 5, 5, GETDATE(), 0, GETDATE(), 0),

-- Phòng Pháp lý
(N'Trưởng phòng pháp lý', 15, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Chuyên viên pháp lý', 15, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Nhân viên pháp lý', 15, 5, 5, GETDATE(), 0, GETDATE(), 0),

-- Phòng Kiểm soát chất lượng
(N'Trưởng phòng kiểm soát chất lượng', 16, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Nhân viên kiểm soát chất lượng', 16, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Chuyên viên đảm bảo chất lượng', 16, 5, 5, GETDATE(), 0, GETDATE(), 0),

-- Phòng Bán hàng
(N'Trưởng phòng bán hàng', 17, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Nhân viên bán hàng', 17, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Nhân viên chăm sóc khách hàng', 17, 5, 5, GETDATE(), 0, GETDATE(), 0),

-- Phòng Thiết kế
(N'Trưởng phòng thiết kế', 18, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Nhà thiết kế đồ họa', 18, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Nhà thiết kế sản phẩm', 18, 5, 5, GETDATE(), 0, GETDATE(), 0),

-- Phòng Dịch vụ kỹ thuật
(N'Trưởng phòng dịch vụ kỹ thuật', 19, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Kỹ thuật viên dịch vụ', 19, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Chuyên viên hỗ trợ kỹ thuật', 19, 5, 5, GETDATE(), 0, GETDATE(), 0),

-- Phòng Đối ngoại
(N'Trưởng phòng đối ngoại', 20, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Nhân viên đối ngoại', 20, 5, 5, GETDATE(), 0, GETDATE(), 0),
(N'Chuyên viên quan hệ quốc tế', 20, 5, 5, GETDATE(), 0, GETDATE(), 0);


DELETE FROM ContractTypes;
DBCC CHECKIDENT (ContractTypes, RESEED, 0)

INSERT INTO ContractTypes(Name, created_at, created_by, updated_at, updated_by)
VALUES (N'Hợp đồng có thời hạn', GETDATE(), 0, GETDATE(), 0),
(N'Hợp đông không thời hạn', GETDATE(), 0, GETDATE(), 0);


DELETE FROM ContractSalaries;
DBCC CHECKIDENT (ContractSalaries, RESEED, 0)

INSERT INTO ContractSalaries(BaseSalary, BaseInsurance, RequiredDays, RequiredHours, WageDaily, WageHourly, Factor, created_at, created_by, updated_at, updated_by)
VALUES
(10000000, 1000000, 22, 176, 454545, 56818, 1.0, GETDATE(), 0, GETDATE(), 0),
(12000000, 1200000, 22, 176, 545454, 68182, 1.1, GETDATE(), 0, GETDATE(), 0),
(11000000, 1100000, 22, 176, 500000, 62500, 1.05, GETDATE(), 0, GETDATE(), 0),
(9500000, 950000, 22, 176, 431818, 53750, 0.95, GETDATE(), 0, GETDATE(), 0),
(13000000, 1300000, 22, 176, 590909, 72727, 1.15, GETDATE(), 0, GETDATE(), 0),
(11500000, 1150000, 22, 176, 522727, 65341, 1.05, GETDATE(), 0, GETDATE(), 0),
(10500000, 1050000, 22, 176, 477273, 59659, 1.0, GETDATE(), 0, GETDATE(), 0),
(12500000, 1250000, 22, 176, 568182, 71023, 1.1, GETDATE(), 0, GETDATE(), 0),
(9800000, 980000, 22, 176, 445455, 55682, 0.98, GETDATE(), 0, GETDATE(), 0),
(13500000, 1350000, 22, 176, 613636, 75795, 1.2, GETDATE(), 0, GETDATE(), 0),
(10200000, 1020000, 22, 176, 463636, 57955, 1.0, GETDATE(), 0, GETDATE(), 0),
(12500000, 1250000, 22, 176, 568182, 71023, 1.1, GETDATE(), 0, GETDATE(), 0),
(8500000, 850000, 22, 176, 386364, 48295, 0.85, GETDATE(), 0, GETDATE(), 0),
(12800000, 1280000, 22, 176, 581818, 72727, 1.1, GETDATE(), 0, GETDATE(), 0),
(9200000, 920000, 22, 176, 418182, 52273, 0.9, GETDATE(), 0, GETDATE(), 0),
(11200000, 1120000, 22, 176, 509091, 63636, 1.05, GETDATE(), 0, GETDATE(), 0),
(14500000, 1450000, 22, 176, 659091, 81818, 1.3, GETDATE(), 0, GETDATE(), 0),
(9800000, 980000, 22, 176, 445455, 55682, 0.98, GETDATE(), 0, GETDATE(), 0),
(12500000, 1250000, 22, 176, 568182, 71023, 1.1, GETDATE(), 0, GETDATE(), 0),
(13500000, 1350000, 22, 176, 613636, 75795, 1.2, GETDATE(), 0, GETDATE(), 0);

DELETE FROM Contracts;
DBCC CHECKIDENT (Contracts, RESEED, 0)
INSERT INTO [dbo].[Contracts]
           ([ContractSalaryId]
           ,[ContractTypeId]
           ,[StartDate]
           ,[EndDate]
           ,[SignDate]
           ,[FireUrlBase]
           ,[FileUrlSigned]
           ,[Name]
           ,[DateOfBirth]
           ,[Gender]
           ,[Address]
           ,[CountrySide]
           ,[NationalID]
           ,[NationalStartDate]
           ,[NationalAddress]
           ,[Level]
           ,[Major]
           ,[PositionId]
           ,[EmployeeSignStatus]
           ,[CompanySignStatus]
           ,[ContractStatus]
           ,[TypeContract]
           ,[created_at]
           ,[updated_at]
           ,[created_by]
           ,[updated_by])
VALUES
(3, 1, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract1', N'contract1_signed', N'Nguyễn Văn An', '1990-01-01', 1, N'Số 1, Đường ABC, Quận 1', N'TP.Hồ Chí Minh', N'123456789', '2020-01-01', N'Địa chỉ quốc gia', N'Đại học', N'Công nghệ thông tin', 12, 1, 2, 1, 1, GETDATE(), GETDATE(), 0, 0),
(4, 2, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract2', N'contract2_signed', N'Lê Minh Tuấn', '1988-03-03', 1, N'Số 2, Đường XYZ, Quận 2', N'Hà Nội', N'987654321', '2021-02-02', N'Địa chỉ quốc gia', N'Thạc sĩ', N'Marketing', 30, 2, 1, 2, 1, GETDATE(), GETDATE(), 0, 0),
(1, 1, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract3', N'contract3_signed', N'Nguyễn Thị Bích', '1992-02-02', 0, N'Số 3, Đường DEF, Quận 3', N'TP.Hồ Chí Minh', N'456789123', '2022-03-03', N'Địa chỉ quốc gia', N'Tiến sĩ', N'Tài chính', 25, 1, 2, 4, 2, GETDATE(), GETDATE(), 0, 0),
(2, 2, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract4', N'contract4_signed', N'Phạm Ngọc Hân', '1991-04-04', 0, N'Số 4, Đường GHI, Quận 4', N'Hà Nội', N'321654987', '2023-04-04', N'Địa chỉ quốc gia', N'Thạc sĩ', N'Phát triển sản phẩm', 15, 2, 1, 3, 1, GETDATE(), GETDATE(), 0, 0),
(5, 1, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract5', N'contract5_signed', N'Võ Văn Hoàng', '1985-05-05', 1, N'Số 5, Đường JKL, Quận 5', N'TP.Hồ Chí Minh', N'654789321', '2024-05-05', N'Địa chỉ quốc gia', N'Kỹ sư', N'Nhân sự', 20, 1, 2, 1, 2, GETDATE(), GETDATE(), 0, 0),
(7, 2, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract6', N'contract6_signed', N'Nguyễn Thị Lan', '1989-08-08', 0, N'Số 6, Đường MNO, Quận 6', N'Hà Nội', N'789123456', '2025-06-06', N'Địa chỉ quốc gia', N'Tiến sĩ', N'Kế toán', 10, 2, 1, 2, 2, GETDATE(), GETDATE(), 0, 0),
(8, 1, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract7', N'contract7_signed', N'Phan Văn Kiệt', '1994-09-09', 1, N'Số 7, Đường PQR, Quận 7', N'TP.Hồ Chí Minh', N'321654789', '2026-07-07', N'Địa chỉ quốc gia', N'Kỹ sư', N'Công nghệ thông tin', 5, 1, 2, 1, 1, GETDATE(), GETDATE(), 0, 0),
(6, 2, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract8', N'contract8_signed', N'Trương Thị Nhung', '1986-10-10', 0, N'Số 8, Đường STU, Quận 8', N'Hà Nội', N'456123789', '2027-08-08', N'Địa chỉ quốc gia', N'Tiến sĩ', N'Kế toán', 8, 2, 1, 3, 2, GETDATE(), GETDATE(), 0, 0),
(9, 1, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract9', N'contract9_signed', N'Nguyễn Hữu Phước', '1991-11-11', 1, N'Số 9, Đường VWX, Quận 9', N'TP.Hồ Chí Minh', N'654321987', '2028-09-09', N'Địa chỉ quốc gia', N'Kỹ sư', N'Điện tử', 12, 1, 1, 1, 1, GETDATE(), GETDATE(), 0, 0),
(10, 2, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract10', N'contract10_signed', N'Lê Thị Thảo', '1985-12-12', 0, N'Số 10, Đường YZ, Quận 10', N'Hà Nội', N'123456321', '2029-10-10', N'Địa chỉ quốc gia', N'Thạc sĩ', N'Marketing', 18, 2, 2, 2, 2, GETDATE(), GETDATE(), 0, 0),
(11, 1, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract11', N'contract11_signed', N'Nguyễn Văn K', '1993-06-06', 1, N'Số 11, Đường QRS, Quận 11', N'TP.Hồ Chí Minh', N'123123123', '2030-11-11', N'Địa chỉ quốc gia', N'Tiến sĩ', N'Kinh tế', 3, 1, 1, 3, 1, GETDATE(), GETDATE(), 0, 0),
(12, 2, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract12', N'contract12_signed', N'Vũ Thị Hương', '1994-07-07', 0, N'Số 12, Đường TUV, Quận 12', N'Hà Nội', N'321321321', '2031-12-12', N'Địa chỉ quốc gia', N'Thạc sĩ', N'Kinh doanh', 22, 2, 2, 4, 2, GETDATE(), GETDATE(), 0, 0),
(13, 1, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract13', N'contract13_signed', N'Phạm Thị Q', '1988-05-05', 1, N'Số 13, Đường ABC, Quận 1', N'TP.Hồ Chí Minh', N'654654654', '2032-01-01', N'Địa chỉ quốc gia', N'Kỹ sư', N'Kỹ thuật', 15, 1, 1, 1, 1, GETDATE(), GETDATE(), 0, 0),
(14, 2, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract14', N'contract14_signed', N'Nguyễn Văn P', '1995-08-08', 0, N'Số 14, Đường DEF, Quận 2', N'Hà Nội', N'789789789', '2033-02-02', N'Địa chỉ quốc gia', N'Tiến sĩ', N'Kinh tế', 28, 2, 2, 2, 2, GETDATE(), GETDATE(), 0, 0),
(15, 1, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract15', N'contract15_signed', N'Võ Văn Hoàng', '1996-09-09', 1, N'Số 15, Đường GHI, Quận 3', N'TP.Hồ Chí Minh', N'321321321', '2034-03-03', N'Địa chỉ quốc gia', N'Kỹ sư', N'Xây dựng', 12, 1, 1, 3, 1, GETDATE(), GETDATE(), 0, 0),
(16, 2, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract16', N'contract16_signed', N'Phan Văn Kiệt', '1990-10-10', 0, N'Số 16, Đường JKL, Quận 4', N'Hà Nội', N'456456456', '2035-04-04', N'Địa chỉ quốc gia', N'Tiến sĩ', N'Tài chính', 8, 2, 2, 4, 2, GETDATE(), GETDATE(), 0, 0),
(17, 1, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract17', N'contract17_signed', N'Nguyễn Thị N', '1991-04-04', 1, N'Số 17, Đường MNO, Quận 5', N'TP.Hồ Chí Minh', N'654654654', '2036-05-05', N'Địa chỉ quốc gia', N'Kỹ sư', N'Công nghệ thông tin', 20, 1, 1, 1, 1, GETDATE(), GETDATE(), 0, 0),
(18, 2, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract18', N'contract18_signed', N'Trương Thị Nhung', '1992-11-11', 0, N'Số 18, Đường PQR, Quận 6', N'Hà Nội', N'987987987', '2037-06-06', N'Địa chỉ quốc gia', N'Tiến sĩ', N'Kinh doanh', 30, 2, 2, 2, 2, GETDATE(), GETDATE(), 0, 0),
(19, 1, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract19', N'contract19_signed', N'Nguyễn Văn An', '1993-12-12', 1, N'Số 19, Đường VWX, Quận 9', N'TP.Hồ Chí Minh', N'654321987', '2038-09-09', N'Địa chỉ quốc gia', N'Kỹ sư', N'Điện tử', 18, 1, 1, 1, 1, GETDATE(), GETDATE(), 0, 0),
(20, 2, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract20', N'contract20_signed', N'Lê Văn T', '1994-07-07', 0, N'Số 20, Đường YZ, Quận 10', N'Hà Nội', N'123123456', '2039-08-08', N'Địa chỉ quốc gia', N'Tiến sĩ', N'Marketing', 5, 2, 2, 2, 2, GETDATE(), GETDATE(), 0, 0),
(15, 1, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract21', N'contract21_signed', N'Nguyễn Thị Kim', '1987-04-04', 1, N'Số 21, Đường ABC, Quận 11', N'TP.Hồ Chí Minh', N'654654654', '2040-04-04', N'Địa chỉ quốc gia', N'Tiến sĩ', N'Kinh tế', 10, 1, 2, 3, 1, GETDATE(), GETDATE(), 0, 0),
(12, 2, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract22', N'contract22_signed', N'Nguyễn Hữu Bình', '1988-01-01', 0, N'Số 22, Đường DEF, Quận 12', N'Hà Nội', N'321321321', '2041-01-01', N'Địa chỉ quốc gia', N'Thạc sĩ', N'Kinh doanh', 20, 2, 1, 2, 2, GETDATE(), GETDATE(), 0, 0),
(13, 1, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract23', N'contract23_signed', N'Vũ Văn Khánh', '1989-05-05', 1, N'Số 23, Đường GHI, Quận 13', N'TP.Hồ Chí Minh', N'654654321', '2042-05-05', N'Địa chỉ quốc gia', N'Tiến sĩ', N'Kỹ thuật', 12, 1, 2, 4, 1, GETDATE(), GETDATE(), 0, 0),
(18, 2, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract24', N'contract24_signed', N'Nguyễn Thị Hương', '1986-06-06', 0, N'Số 24, Đường JKL, Quận 14', N'Hà Nội', N'123456789', '2043-06-06', N'Địa chỉ quốc gia', N'Tiến sĩ', N'Phát triển sản phẩm', 15, 2, 1, 3, 2, GETDATE(), GETDATE(), 0, 0),
(17, 1, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract25', N'contract25_signed', N'Phan Văn Sơn', '1995-03-03', 1, N'Số 25, Đường MNO, Quận 15', N'TP.Hồ Chí Minh', N'654321654', '2044-03-03', N'Địa chỉ quốc gia', N'Kỹ sư', N'Điện tử', 14, 1, 1, 2, 1, GETDATE(), GETDATE(), 0, 0),
(19, 2, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract26', N'contract26_signed', N'Lê Thị Thanh', '1994-02-02', 0, N'Số 26, Đường PQR, Quận 16', N'Hà Nội', N'321654987', '2045-02-02', N'Địa chỉ quốc gia', N'Thạc sĩ', N'Kế toán', 16, 2, 2, 1, 2, GETDATE(), GETDATE(), 0, 0),
(20, 1, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract27', N'contract27_signed', N'Nguyễn Văn H', '1993-11-11', 1, N'Số 27, Đường STU, Quận 17', N'TP.Hồ Chí Minh', N'654123789', '2046-11-11', N'Địa chỉ quốc gia', N'Kỹ sư', N'Nhân sự', 5, 1, 1, 1, 1, GETDATE(), GETDATE(), 0, 0),
(3, 1, GETDATE(), DATEADD(year, 1, GETDATE()), GETDATE(), N'contract27', N'contract27_signed', N'Nguyễn Thành Hưng', '2003-02-13', 1, N'Số 27, Đường STU, Quận 17', N'TP.Hồ Chí Minh', N'654123789', '2046-11-11', N'Địa chỉ quốc gia', N'Kỹ sư', N'Nhân sự', 5, 1, 1, 1, 1, GETDATE(), GETDATE(), 0, 0);
DELETE FROM Employees;
DBCC CHECKIDENT (Employees, RESEED, 0)
INSERT INTO [dbo].[Employees]
           ([ContractId]
           ,[PhoneNumber]
           ,[Email]
           ,[UserName]
           ,[Password]
           ,[StatusEmployee]
           ,[Avatar]
           ,[created_at]
           ,[updated_at]
           ,[created_by]
           ,[updated_by])
VALUES
(1, N'0866058725', N'phucminhbeos@gmail.com', N'PhucLa', N'$2a$12$klwPgWXc/Ga3DQ0Cgvgr5uPxRYJajtxequqKIoPOjSc571CibWAE6', 1, N'avatar1.png', GETDATE(), GETDATE(), 0, 0),
(2, N'0912345678', N'employee2@example.com', N'user2', N'password2', 1, N'avatar2.png', GETDATE(), GETDATE(), 0, 0),
(3, N'0901112233', N'employee3@example.com', N'user3', N'password3', 1, N'avatar3.png', GETDATE(), GETDATE(), 0, 0),
(4, N'0922334455', N'employee4@example.com', N'user4', N'password4', 1, N'avatar4.png', GETDATE(), GETDATE(), 0, 0),
(5, N'0933445566', N'employee5@example.com', N'user5', N'password5', 1, N'avatar5.png', GETDATE(), GETDATE(), 0, 0),
(6, N'0944556677', N'employee6@example.com', N'user6', N'password6', 1, N'avatar6.png', GETDATE(), GETDATE(), 0, 0),
(7, N'0955667788', N'employee7@example.com', N'user7', N'password7', 1, N'avatar7.png', GETDATE(), GETDATE(), 0, 0),
(8, N'0966778899', N'employee8@example.com', N'user8', N'password8', 1, N'avatar8.png', GETDATE(), GETDATE(), 0, 0),
(9, N'0977889900', N'employee9@example.com', N'user9', N'password9', 1, N'avatar9.png', GETDATE(), GETDATE(), 0, 0),
(10, N'0988990011', N'employee10@example.com', N'user10', N'password10', 1, N'avatar10.png', GETDATE(), GETDATE(), 0, 0),
(11, N'0911112222', N'employee11@example.com', N'user11', N'password11', 1, N'avatar11.png', GETDATE(), GETDATE(), 0, 0),
(12, N'0922223333', N'employee12@example.com', N'user12', N'password12', 1, N'avatar12.png', GETDATE(), GETDATE(), 0, 0),
(13, N'0933334444', N'employee13@example.com', N'user13', N'password13', 1, N'avatar13.png', GETDATE(), GETDATE(), 0, 0),
(14, N'0944445555', N'employee14@example.com', N'user14', N'password14', 1, N'avatar14.png', GETDATE(), GETDATE(), 0, 0),
(15, N'0955556666', N'employee15@example.com', N'user15', N'password15', 1, N'avatar15.png', GETDATE(), GETDATE(), 0, 0),
(16, N'0966667777', N'employee16@example.com', N'user16', N'password16', 1, N'avatar16.png', GETDATE(), GETDATE(), 0, 0),
(17, N'0977778888', N'employee17@example.com', N'user17', N'password17', 1, N'avatar17.png', GETDATE(), GETDATE(), 0, 0),
(18, N'0988889999', N'employee18@example.com', N'user18', N'password18', 1, N'avatar18.png', GETDATE(), GETDATE(), 0, 0),
(19, N'0912345670', N'employee19@example.com', N'user19', N'password19', 1, N'avatar19.png', GETDATE(), GETDATE(), 0, 0),
(20, N'0923456781', N'employee20@example.com', N'user20', N'password20', 1, N'avatar20.png', GETDATE(), GETDATE(), 0, 0),
(21, N'0934567892', N'employee21@example.com', N'user21', N'password21', 1, N'avatar21.png', GETDATE(), GETDATE(), 0, 0),
(22, N'0945678903', N'employee22@example.com', N'user22', N'password22', 1, N'avatar22.png', GETDATE(), GETDATE(), 0, 0),
(23, N'0956789014', N'employee23@example.com', N'user23', N'password23', 1, N'avatar23.png', GETDATE(), GETDATE(), 0, 0),
(24, N'0967890125', N'employee24@example.com', N'user24', N'password24', 1, N'avatar24.png', GETDATE(), GETDATE(), 0, 0),
(25, N'0978901236', N'employee25@example.com', N'user25', N'password25', 1, N'avatar25.png', GETDATE(), GETDATE(), 0, 0),
(26, N'0989012347', N'employee26@example.com', N'user26', N'password26', 1, N'avatar26.png', GETDATE(), GETDATE(), 0, 0),
(27, N'0910123458', N'employee27@example.com', N'user27', N'password27', 1, N'avatar27.png', GETDATE(), GETDATE(), 0, 0),
(28, N'0946928815', N'thanh.hung.st302@gmail.com', N'hugnt', N'123456', 1, N'avatar27.png', GETDATE(), GETDATE(), 0, 0);


DELETE FROM Bonus;
DBCC CHECKIDENT (Bonus, RESEED, 0)
INSERT INTO [dbo].[Bonus]
           ([ParameterName]
           ,[Amount]
           ,[Name]
           ,[created_at]
           ,[updated_at]
           ,[created_by]
           ,[updated_by])
VALUES
           ('PARAM_BONUS_NHAN_VIEN_XUAT_SAC', 5000000, N'Thưởng Nhân Viên Xuất Sắc', GETDATE(), GETDATE(), 1, 1),
           ('PARAM_BONUS_DU_AN_THANH_CONG', 3000000, N'Thưởng Dự Án Thành Công', GETDATE(), GETDATE(), 1, 1),
           ('PARAM_BONUS_TET_NGUYEN_DAN', 2000000, N'Thưởng Tết Nguyên Đán', GETDATE(), GETDATE(), 1, 1),
           ('PARAM_BONUS_HOAN_THANH_KPI', 7000000, N'Thưởng Hoàn Thành KPI', GETDATE(), GETDATE(), 1, 1),
           ('PARAM_BONUS_DOT_XUAT', 4000000, N'Thưởng Đột Xuất', GETDATE(), GETDATE(), 1, 1),
           ('PARAM_BONUS_KHACH_HANG_MOI', 2500000, N'Thưởng Khách Hàng Mới', GETDATE(), GETDATE(), 1, 1),
           ('PARAM_BONUS_CONG_HIEN', 6000000, N'Thưởng Cống Hiến', GETDATE(), GETDATE(), 1, 1),
           ('PARAM_BONUS_DE_XUAT_SANG_KIEN', 3500000, N'Thưởng Đề Xuất Sáng Kiến', GETDATE(), GETDATE(), 1, 1),
           ('PARAM_BONUS_NHAN_VIEN_GUONG_MAU', 4500000, N'Thưởng Nhân Viên Gương Mẫu', GETDATE(), GETDATE(), 1, 1),
           ('PARAM_BONUS_DAT_DOANH_THU', 5500000, N'Thưởng Đạt Doanh Thu', GETDATE(), GETDATE(), 1, 1),
           ('PARAM_BONUS_THAM_GIA_KHOA_HOC', 1500000, N'Thưởng Tham Gia Khóa Học', GETDATE(), GETDATE(), 1, 1),
           ('PARAM_BONUS_DANH_GIA_XUAT_SAC', 5000000, N'Thưởng Đánh Giá Xuất Sắc', GETDATE(), GETDATE(), 1, 1),
           ('PARAM_BONUS_CAI_TIEN_QUY_TRINH', 3000000, N'Thưởng Cải Tiến Quy Trình', GETDATE(), GETDATE(), 1, 1),
           ('PARAM_BONUS_LAM_THEM_GIO', 2000000, N'Thưởng Làm Thêm Giờ', GETDATE(), GETDATE(), 1, 1),
           ('PARAM_BONUS_BAO_TRI_HE_THONG', 2500000, N'Thưởng Bảo Trì Hệ Thống', GETDATE(), GETDATE(), 1, 1);

		   
DELETE FROM Deductions;
DBCC CHECKIDENT (Deductions, RESEED, 0)
INSERT INTO [dbo].[Deductions]
           ([Name]
           ,[Amount]
           ,[ParameterName]
           ,[created_at]
           ,[updated_at]
           ,[created_by]
           ,[updated_by])
VALUES
           (N'Khấu Trừ Vi Phạm Giờ Làm', 2000000, 'PARAM_DEDUCTION_VI_PHAM_GIO_LAM', GETDATE(), GETDATE(), 1, 1),
           (N'Khấu Trừ Vi Phạm Kỷ Luật', 3000000, 'PARAM_DEDUCTION_VI_PHAM_KY_LUAT', GETDATE(), GETDATE(), 1, 1),
           (N'Khấu Trừ Vi Phạm Quy Trình', 1500000, 'PARAM_DEDUCTION_VI_PHAM_QUY_TRINH', GETDATE(), GETDATE(), 1, 1),
           (N'Khấu Trừ Vi Phạm Nội Quy', 2500000, 'PARAM_DEDUCTION_VI_PHAM_NOI_QUY', GETDATE(), GETDATE(), 1, 1),
           (N'Khấu Trừ Vi Phạm An Toàn', 1800000, 'PARAM_DEDUCTION_VI_PHAM_AN_TOAN', GETDATE(), GETDATE(), 1, 1),
           (N'Khấu Trừ Vi Phạm Hành Vi Ứng Xử', 2200000, 'PARAM_DEDUCTION_VI_PHAM_HANH_VI', GETDATE(), GETDATE(), 1, 1),
           (N'Khấu Trừ Vi Phạm Quy Định Đeo Thẻ', 1600000, 'PARAM_DEDUCTION_VI_PHAM_DEO_THE', GETDATE(), GETDATE(), 1, 1),
           (N'Khấu Trừ Vi Phạm Đến Muộn', 1400000, 'PARAM_DEDUCTION_VI_PHAM_DEN_MUON', GETDATE(), GETDATE(), 1, 1),
           (N'Khấu Trừ Vi Phạm Sử Dụng Điện Thoại', 1700000, 'PARAM_DEDUCTION_VI_PHAM_DIEN_THOAI', GETDATE(), GETDATE(), 1, 1),
           (N'Khấu Trừ Vi Phạm Sử Dụng Thiết Bị', 1900000, 'PARAM_DEDUCTION_VI_PHAM_THIET_BI', GETDATE(), GETDATE(), 1, 1),
           (N'Khấu Trừ Vi Phạm Đạo Đức Nghề Nghiệp', 2100000, 'PARAM_DEDUCTION_VI_PHAM_DAO_DUC', GETDATE(), GETDATE(), 1, 1),
           (N'Khấu Trừ Vi Phạm Không Tham Gia Họp', 1300000, 'PARAM_DEDUCTION_VI_PHAM_KHONG_THAM_GIA', GETDATE(), GETDATE(), 1, 1),
           (N'Khấu Trừ Vi Phạm Chiếm Dụng Không Gian', 1500000, 'PARAM_DEDUCTION_VI_PHAM_CHIEM_DUNG', GETDATE(), GETDATE(), 1, 1),
           (N'Khấu Trừ Vi Phạm Không Thực Hiện Nhiệm Vụ', 3000000, 'PARAM_DEDUCTION_VI_PHAM_KHONG_THUC_HIEN', GETDATE(), GETDATE(), 1, 1),
           (N'Khấu Trừ Vi Phạm Không Chấp Hành Lệnh', 2800000, 'PARAM_DEDUCTION_VI_PHAM_KHONG_CHAP_HANH', GETDATE(), GETDATE(), 1, 1);

DELETE FROM Fomulas;
DBCC CHECKIDENT (Fomulas, RESEED, 0)

INSERT INTO Fomulas(Name,ParameterName, FomulaDetail, Note,created_at, created_by, updated_at, updated_by)
VALUES 
(N'Lương thời gian','FORMULA_LUONG_THOI_GIAN','[PARAM_BASE_SALARY]*[PARAM_REAL_HOURS]/[PARAM_BASE_HOURS]',N'', GETDATE(), 0, GETDATE(), 0),
(N'Phụ cấp thực nhận','FORMULA_PHU_CAP','[PARAM_TOTAL_ALLOWANCE]*[PARAM_REAL_HOURS]/[PARAM_BASE_HOURS]',N'', GETDATE(), 0, GETDATE(), 0),
(N'Tổng thu nhập','FORMULA_TONG_THU_NHAP','[FORMULA_LUONG_THOI_GIAN]+[FORMULA_PHU_CAP]+[PARAM_TOTAL_BONUS]',N'', GETDATE(), 0, GETDATE(), 0),
(N'Tổng khấu trừ (cố định)','FORMULA_TONG_KHAU_TRU','[PARAM_TOTAL_INSURANCE]+[PARAM_TOTAL_PESONAL_TAX_DEDUCTION]',N'', GETDATE(), 0, GETDATE(), 0),
(N'Các khoản không tính thuế','FORMULA_KHONG_THUE','[PARAM_TOTAL_ADVANCE]+[PARAM_ALLOWANCE_LUNCH_NOTAX]-[PARAM_TOTAL_DEDUCTION_NOTAX]',N'', GETDATE(), 0, GETDATE(), 0),
(N'CT tính lương quy chuẩn','FORMULA_QUY_CHUAN','[FORMULA_TONG_THU_NHAP]-[PARAM_TOTAL_TAX]-[PARAM_TOTAL_INSURANCE]+[FORMULA_KHONG_THUE]',N'', GETDATE(), 0, GETDATE(), 0);


DELETE FROM Insurances;
DBCC CHECKIDENT (Insurances, RESEED, 0)

INSERT INTO Insurances(Name,PercentEmployee, PercentCompany, ParameterName,created_at, created_by, updated_at, updated_by)
VALUES 
(N'BHXH',0.08, 0.175 ,N'PARAM_INSURANCE_BHXH', GETDATE(), 0, GETDATE(), 0),
(N'BHYT',0.015, 0.03 ,N'PARAM_INSURANCE_BHYT', GETDATE(), 0, GETDATE(), 0),
(N'BHTN',0.01, 0.01 ,N'PARAM_INSURANCE_BHTN', GETDATE(), 0, GETDATE(), 0);


DELETE FROM Allowances;
DBCC CHECKIDENT (Allowances, RESEED, 0)

INSERT INTO Allowances(Name, Amount, Terms, ParameterName, created_at, created_by, updated_at, updated_by)
VALUES 
(N'Phụ cấp ăn trưa', 50000, N'', N'PARAMS_ALLOWANCE_PC_AN_TRUA', GETDATE(), 0, GETDATE(), 0),
(N'Phụ cấp xăng xe', 30000, N'', N'PARAMS_ALLOWANCE_PC_XANG_XE', GETDATE(), 0, GETDATE(), 0),
(N'Phụ cấp nhà ở', 100000, N'', N'PARAMS_ALLOWANCE_PC_NHA_O', GETDATE(), 0, GETDATE(), 0),
(N'Phụ cấp điện thoại', 20000, N'', N'PARAMS_ALLOWANCE_PC_DIEN_THOAI', GETDATE(), 0, GETDATE(), 0),
(N'Phụ cấp đi công tác', 70000, N'', N'PARAMS_ALLOWANCE_PC_DI_CONG_TAC', GETDATE(), 0, GETDATE(), 0),
(N'Phụ cấp đào tạo', 150000, N'', N'PARAMS_ALLOWANCE_PC_DAO_TAO', GETDATE(), 0, GETDATE(), 0),
(N'Phụ cấp bảo hiểm', 80000, N'', N'PARAMS_ALLOWANCE_PC_BAO_HIEM', GETDATE(), 0, GETDATE(), 0),
(N'Phụ cấp thâm niên', 25000, N'', N'PARAMS_ALLOWANCE_PC_THAM_NIEN', GETDATE(), 0, GETDATE(), 0),
(N'Phụ cấp trách nhiệm', 60000, N'', N'PARAMS_ALLOWANCE_PC_TRACH_NHIEM', GETDATE(), 0, GETDATE(), 0),
(N'Phụ cấp khu vực', 90000, N'', N'PARAMS_ALLOWANCE_PC_KHU_VUC', GETDATE(), 0, GETDATE(), 0);

DELETE FROM Payrolls;
DBCC CHECKIDENT (Payrolls, RESEED, 0)

INSERT INTO Payrolls(Year,Month, EmployeeId, ContractId, OtherBonus, OtherDeduction,FomulaId, created_at, created_by, updated_at, updated_by)
VALUES 
(2024,10, 1, 1, 0, 0, 6, GETDATE(), 0, GETDATE(), 0),
(2024,10, 2, 2, 10000, 0, 6, GETDATE(), 0, GETDATE(), 0),
(2024,10, 3, 3, 20000, 0, 6, GETDATE(), 0, GETDATE(), 0),
(2024,10, 4, 4, 0, 10000, 6, GETDATE(), 0, GETDATE(), 0),
(2024,10, 5, 5, 0, 10000, 6, GETDATE(), 0, GETDATE(), 0),
(2024,10, 6, 6, 50000, 20000, 6, GETDATE(), 0, GETDATE(), 0);

SELECT * FROM Payrolls

DELETE FROM BonusDetails;
DBCC CHECKIDENT (BonusDetails, RESEED, 0)
INSERT INTO BonusDetails(PayrollId, BonusId, Factor, created_at, created_by, updated_at, updated_by)
VALUES 
(1, 1, 1, GETDATE(), 0, GETDATE(), 0),
(1, 2, 1, GETDATE(), 0, GETDATE(), 0),
(1, 3, 1, GETDATE(), 0, GETDATE(), 0),
(1, 4, 1, GETDATE(), 0, GETDATE(), 0),
(2, 4, 1, GETDATE(), 0, GETDATE(), 0),
(2, 5, 1, GETDATE(), 0, GETDATE(), 0),
(3, 1, 1, GETDATE(), 0, GETDATE(), 0),
(4, 8, 1, GETDATE(), 0, GETDATE(), 0),
(5, 1, 1, GETDATE(), 0, GETDATE(), 0),
(5, 6, 1, GETDATE(), 0, GETDATE(), 0),
(5, 4, 1, GETDATE(), 0, GETDATE(), 0);

DELETE FROM DeductionDetails;
DBCC CHECKIDENT (DeductionDetails, RESEED, 0)
INSERT INTO DeductionDetails(PayrollId, DeductionId, Factor, created_at, created_by, updated_at, updated_by)
VALUES 
(1, 2, 1, GETDATE(), 0, GETDATE(), 0),
(1, 4, 1, GETDATE(), 0, GETDATE(), 0),
(1, 6, 1, GETDATE(), 0, GETDATE(), 0),
(2, 4, 1, GETDATE(), 0, GETDATE(), 0),
(2, 6, 1, GETDATE(), 0, GETDATE(), 0),
(3, 4, 1, GETDATE(), 0, GETDATE(), 0),
(4, 4, 1, GETDATE(), 0, GETDATE(), 0),
(5, 3, 1, GETDATE(), 0, GETDATE(), 0),
(5, 4, 1, GETDATE(), 0, GETDATE(), 0),
(5, 7, 1, GETDATE(), 0, GETDATE(), 0);

DELETE FROM TaxDeductions;
DBCC CHECKIDENT (TaxDeductions, RESEED, 0)
INSERT INTO TaxDeductions(Name, Amount, Terms, ParameterName, created_at, created_by, updated_at, updated_by)
VALUES 
(N'Giảm trừ bản thân', 11000000, N'Áp dụng cho tất cả các cá nhân đóng thuế', N'PARAM_TAXDEDUCTION_PERSONAL', GETDATE(), 1, GETDATE(), 0),
(N'Giảm trừ người phụ thuộc', 4400000, N'Áp dụng cho người lao động có con nhỏ', N'PARAM_TAXDEDUCTION_DEPENDENCY', GETDATE(), 1, GETDATE(), 0);

DELETE FROM TaxDeductionDetails;
DBCC CHECKIDENT (TaxDeductionDetails, RESEED, 0)
INSERT INTO TaxDeductionDetails(EmployeeId, TaxDeductionId, Factor, created_at, created_by, updated_at, updated_by)
VALUES 
(1, 1, 1, GETDATE(), 0, GETDATE(), 0),
(1, 2, 2, GETDATE(), 0, GETDATE(), 0),
(2, 1, 1, GETDATE(), 0, GETDATE(), 0),
(2, 2, 1, GETDATE(), 0, GETDATE(), 0),
(3, 1, 1, GETDATE(), 0, GETDATE(), 0),
(4, 1, 1, GETDATE(), 0, GETDATE(), 0),
(5, 1, 1, GETDATE(), 0, GETDATE(), 0),
(6, 1, 1, GETDATE(), 0, GETDATE(), 0);

DELETE FROM TaxRates;
DBCC CHECKIDENT (TaxRates, RESEED, 0)
INSERT INTO TaxRates(Name, MinTaxIncome,MaxTaxIncome, [Percent], MinusAmount,ParameterName,Condition, created_at, created_by, updated_at, updated_by)
VALUES 
(N'Đến 5 triệu đồng (trđ)',0,10000000, 0.05,0,'PARAM_TAXRATE_5',N'Thu nhập tính thuế /tháng > 5 triệu đồng (trđ)' , GETDATE(), 0, GETDATE(), 0),
(N'Trên 5 trđ đến 10 trđ',5000000,10000000, 0.10,0.25*1000000,'PARAM_TAXRATE_10',N'Thu nhập tính thuế /tháng > 5 triệu đồng (trđ) và <= 10 triệu đồng (trđ)', GETDATE(), 0, GETDATE(), 0),
(N'Trên 10 trđ đến 18 trđ', 10000000, 18000000, 0.15, 0.75*1000000, 'PARAM_TAXRATE_15',N'Thu nhập tính thuế /tháng > 10 triệu đồng (trđ) và <= 18 triệu đồng (trđ)', GETDATE(), 0, GETDATE(), 0),
(N'Trên 18 trđ đến 32 trđ', 18000000, 32000000, 0.20, 1.65*1000000,'PARAM_TAXRATE_20',N'Thu nhập tính thuế /tháng > 18 triệu đồng (trđ) và <= 32 triệu đồng (trđ)', GETDATE(), 0, GETDATE(), 0),
(N'Trên 32 trđ đến 52 trđ', 32000000, 52000000, 0.25, 3.25*1000000,'PARAM_TAXRATE_25',N'Thu nhập tính thuế /tháng > 32 triệu đồng (trđ) và <= 52 triệu đồng (trđ)', GETDATE(), 0, GETDATE(), 0),
(N'Trên 52 trđ đến 80 trđ', 52000000, 80000000, 0.30, 5.85*1000000, 'PARAM_TAXRATE_30',N'Thu nhập tính thuế /tháng > 52 triệu đồng (trđ) và <= 80 triệu đồng (trđ)', GETDATE(), 0, GETDATE(), 0),
(N'Trên 80 trđ', 80000000, 999999999999999, 0.35, 9.85*1000000,'PARAM_TAXRATE_35',N'Thu nhập tính thuế /tháng > 80 triệu đồng (trđ)', GETDATE(), 0, GETDATE(), 0);


DELETE FROM ContractInsurances;
DBCC CHECKIDENT (ContractInsurances, RESEED, 0)
INSERT INTO ContractInsurances(ContractId, InsuranceId, created_at, created_by, updated_at, updated_by)
VALUES 
(1, 1, GETDATE(), 0, GETDATE(), 0),
(1, 2, GETDATE(), 0, GETDATE(), 0),
(1, 3, GETDATE(), 0, GETDATE(), 0),
(3, 1, GETDATE(), 0, GETDATE(), 0),
(3, 2, GETDATE(), 0, GETDATE(), 0),
(3, 3, GETDATE(), 0, GETDATE(), 0);

SELECT * FROM ContractAllowances

DELETE FROM ContractAllowances;
INSERT INTO ContractAllowances(ContractId, AllowanceId, created_at, created_by, updated_at, updated_by)
VALUES 
(1, 1, GETDATE(), 0, GETDATE(), 0),
(1, 4, GETDATE(), 0, GETDATE(), 0),
(1, 5, GETDATE(), 0, GETDATE(), 0),
(2, 2, GETDATE(), 0, GETDATE(), 0),
(2, 4, GETDATE(), 0, GETDATE(), 0),
(2, 7, GETDATE(), 0, GETDATE(), 0),
(3, 1, GETDATE(), 0, GETDATE(), 0),
(3, 4, GETDATE(), 0, GETDATE(), 0),
(3, 10, GETDATE(), 0, GETDATE(), 0);

SELECT * FROM TaxDeductionDetails ;